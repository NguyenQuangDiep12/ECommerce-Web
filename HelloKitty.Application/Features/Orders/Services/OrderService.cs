using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Orders.DTOs;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Entities;
using HelloKitty.Domain.Inventory.Enums;
using HelloKitty.Domain.Orders.Entities;
using HelloKitty.Domain.Orders.Enums;
using HelloKitty.Domain.Promotions.Enums;

namespace HelloKitty.Application.Features.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public OrderService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }

        public async Task<Result> CancelOrderAsync(Guid orderId, Guid userId, CancellationToken ct = default)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, ct);

            if (order == null)
                return Result.Failure("Order not found");

            if (order.UserId != userId)
                return Result.Failure("authority not permitted");

            if (order.OrderStatus != OrderStatus.Pending)
                return Result.Failure("Only pending status order can be cancelled");

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                order.OrderStatus = OrderStatus.Cancelled;

                foreach (var item in order.OrderItems)
                {
                    item.ProductVariant.Quantity += (int)item.Quantity;

                    await _unitOfWork.InventoriesLogs.AddAsync(new InventoryLog
                    {
                        VariantId = item.VariantId,
                        ChangeType = ChangeType.Refund,
                        QuantityChange = (int)item.Quantity,
                        CurrentStock = item.ProductVariant.Quantity
                    }, ct);
                }

                _unitOfWork.Orders.Update(order);

                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitAsync();

                return Result.Success();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return Result.Failure("Order Cancellation Failed");
            }
        }

        public async Task<Result<OrderDetailResponse>> CreateFromCartAsync(
            Guid userId, CreateOrderRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<OrderDetailResponse>.ValidationFailure(errors);

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);
            if (cart == null || cart.CartItems.Count == 0)
                return Result<OrderDetailResponse>.Failure("Empty Cart");

            foreach (var item in cart.CartItems)
            {
                var variant = item.ProductVariant;

                if (variant == null || !variant.IsActive)
                    return Result<OrderDetailResponse>.Failure("The product is unavailable");

                if (variant.Quantity < item.Quantity)
                    return Result<OrderDetailResponse>.Failure(
                        $"{variant.Product.ProductName} product are out of stock");
            }

            decimal totalAmount = cart.CartItems.Sum(x => x.ProductVariant.Price * x.Quantity);
            decimal discountAmount = 0;

            if (!string.IsNullOrWhiteSpace(request.VoucherCode))
            {
                var voucherResult = await ApplyVoucherAsync(
                    request.VoucherCode, userId, totalAmount, ct);

                if (!voucherResult.IsSuccess)
                    return Result<OrderDetailResponse>.Failure(voucherResult.Error!);

                discountAmount = voucherResult.Value;
            }

            decimal finalAmount = Math.Max(0, totalAmount - discountAmount);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var order = new Order
                {
                    UserId = userId,
                    OrderStatus = OrderStatus.Pending,
                    TotalAmount = totalAmount,
                    FinalAmount = finalAmount,
                    CreatedAt = DateTime.UtcNow,
                    Address = new OrderAddress
                    {
                        ReceiverName = request.ReceiverName,
                        Phone = request.Phone,
                        Province = request.Province,
                        District = request.District,
                        Ward = request.Ward,
                        Street = request.Street
                    }
                };

                foreach (var item in cart.CartItems)
                {
                    var variant = item.ProductVariant;

                    order.OrderItems.Add(new OrderItem
                    {
                        VariantId = item.VariantId,
                        Quantity = (uint)item.Quantity,
                        UnitPrice = variant.Price
                    });

                    variant.Quantity -= item.Quantity;

                    await _unitOfWork.InventoriesLogs.AddAsync(new InventoryLog
                    {
                        VariantId = variant.VariantId,
                        ChangeType = ChangeType.Order,
                        QuantityChange = -item.Quantity,
                        CurrentStock = variant.Quantity
                    }, ct);
                }

                order.Payments.Add(new Payment
                {
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = PaymentStatus.Pending,
                    Amount = finalAmount
                });

                await _unitOfWork.Orders.AddAsync(order, ct);

                cart.CartItems.Clear();
                _unitOfWork.Carts.Update(cart);

                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitAsync();

                var created = await _unitOfWork.Orders.GetByIdWithDetailsAsync(order.OrderId, ct);

                return Result<OrderDetailResponse>.Success(MapToDetailResponse(created!));
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return Result<OrderDetailResponse>.Failure("Order Creation Failed");
            }
        }

        public async Task<Result<OrderDetailResponse>> GetByIdAsync(Guid orderId, Guid userId, CancellationToken ct = default)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, ct);

            if (order == null)
                return Result<OrderDetailResponse>.Failure("Order Not Found");

            if (order.UserId != userId)
                return Result<OrderDetailResponse>.Failure("Authority not permitted");

            return Result<OrderDetailResponse>.Success(MapToDetailResponse(order));
        }

        public async Task<Result<PagedResult<OrderListResponse>>> GetUserOrdersAsync(
            Guid userId, int page, int pageSize, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Orders.GetByUserIdPagedAsync(userId, page, pageSize, ct);
            return Result<PagedResult<OrderListResponse>>.Success(MapToListPaged(paged));
        }

        public async Task<Result> UpdateStatusAsync(
            Guid orderId, UpdateOrderStatusRequest request, CancellationToken ct = default)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, ct);

            if (order == null)
                return Result.Failure("Order Not Found");

            if (order.OrderStatus == OrderStatus.Cancelled ||
                order.OrderStatus == OrderStatus.Complete)
                return Result.Failure("Unable to update status");

            order.OrderStatus = request.Status;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
        // For Admin's Permission
        public async Task<Result<OrderDetailResponse>> GetByIdForAdminAsync(Guid orderId, CancellationToken ct = default)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, ct);

            if (order == null)
                return Result<OrderDetailResponse>.Failure("Order not found");

            return Result<OrderDetailResponse>.Success(MapToDetailResponse(order));
        }

        public async Task<Result<PagedResult<OrderListResponse>>> GetAllOrdersAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Orders.GetAllPagedAsync(page, pageSize, ct);
            return Result<PagedResult<OrderListResponse>>.Success(MapToListPaged(paged));
        }

        public async Task<Result<ShipmentDetailResponse>> CreateShipmentAsync(Guid orderId, CreateShipmentRequest request, CancellationToken ct = default)
        {
            var order = await _unitOfWork.Orders.GetByIdWithDetailsAsync(orderId, ct);

            if (order == null)
                return Result<ShipmentDetailResponse>.Failure("Order not found");

            if (order.Shipment != null)
                return Result<ShipmentDetailResponse>.Failure("A shipment already exists for this order");

            if (order.OrderStatus != OrderStatus.Paid && order.OrderStatus != OrderStatus.Processing)
                return Result<ShipmentDetailResponse>.Failure("Shipments can only be created for orders that have been paid");

            var shipment = new Shipment
            {
                OrderId = orderId,
                ShipmentProvider = request.ShipmentProvider,
                TrackingCode = request.TrackingCode,
                ShipmentStatus = ShipmentStatus.Pending
            };

            order.Shipment = shipment;
            order.OrderStatus = OrderStatus.Shipped;

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<ShipmentDetailResponse>.Success(new ShipmentDetailResponse
            {
                ShipmentId = shipment.ShipmentId,
                ShipmentProvider = shipment.ShipmentProvider,
                TrackingCode = shipment.TrackingCode,
                ShipmentStatus = shipment.ShipmentStatus
            });
        }

        private async Task<Result<decimal>> ApplyVoucherAsync(
            string code, Guid userId, decimal totalAmount, CancellationToken ct)
        {
            var voucher = await _unitOfWork.Vouchers.GetByCodeAsync(code, ct);

            if (voucher == null || !voucher.IsActive)
                return Result<decimal>.Failure("Invalid code");

            if (DateTime.UtcNow < voucher.StartDate || DateTime.UtcNow > voucher.EndDate)
                return Result<decimal>.Failure("Voucher has expired");

            if (totalAmount < voucher.MinOrderAmount)
                return Result<decimal>.Failure("Not eligible");

            decimal discount = voucher.DiscountType == DiscountType.Percentage
                ? Math.Min(totalAmount * voucher.DiscountValue / 100,
                           voucher.MaxDiscountAmount ?? decimal.MaxValue)
                : voucher.DiscountValue;

            return Result<decimal>.Success(discount);
        }
        private static PagedResult<OrderListResponse> MapToListPaged(PagedResult<Order> paged)
        {
            return new PagedResult<OrderListResponse>
            {
                Items = paged.Items.Select(o => new OrderListResponse {
                    OrderId = o.OrderId,
                    Status = o.OrderStatus,
                    TotalAmount = o.TotalAmount,
                    FinalAmount = o.FinalAmount,
                    DiscountAmount = o.TotalAmount - o.FinalAmount,
                    ItemCount = o.OrderItems.Count,
                    CreatedAt = o.CreatedAt}),

                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            };
        }

        private static OrderDetailResponse MapToDetailResponse(Order o)
        {
            return new OrderDetailResponse
            {
                OrderId = o.OrderId,
                Status = o.OrderStatus,
                TotalAmount = o.TotalAmount,
                FinalAmount = o.FinalAmount,
                DiscountAmount = o.TotalAmount - o.FinalAmount,
                CreatedAt = o.CreatedAt,

                Address = new OrderAddressResponse
                {
                    ReceiverName = o.Address.ReceiverName,
                    Phone = o.Address.Phone,
                    Province = o.Address.Province,
                    District = o.Address.District,
                    Ward = o.Address.Ward,
                    Street = o.Address.Street
                },

                Items = o.OrderItems.Select(i => new OrderItemResponse
                {
                    OrderItemId = i.OrderItemId,
                    ProductName = i.ProductVariant.Product.ProductName,
                    SKU = i.ProductVariant.SKU,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    SubTotal = i.UnitPrice * i.Quantity
                }).ToList(),

                Payment = o.Payments.FirstOrDefault() is { } p // if o.Payments.FirstOrDefault()  != null => var p = o.Payments.FirstOrDefault() 
                    ? new PaymentResponse
                    {
                        PaymentId = p.PaymentId,
                        PaymentMethod = p.PaymentMethod.ToString(),
                        Status = p.PaymentStatus,
                        Amount = p.Amount
                    }
                    : null,

                            Shipment = o.Shipment is { } s // if o.Shipment != null => var s = o.Shipment
                    ? new ShipmentResponse
                    {
                        ShipmentId = s.ShipmentId,
                        Status = s.ShipmentStatus,
                        TrackingCode = s.TrackingCode
                    }
                    : null
            };
        }
    }
}