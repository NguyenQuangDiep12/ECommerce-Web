using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Carts.Services;
using HelloKitty.Application.Features.Carts.DTOs;
using HelloKitty.Domain.Carts.Entities;
using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloKitty.Domain.Catalog.Entities;

namespace HelloKitty.Application.Features.Carts.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public CartService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }

        public async Task<Result<CartResponse>> AddItemAsync(Guid userId, AddCartItemRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if(errors.Count > 0)
            {
                return Result<CartResponse>.ValidationFailure(errors);
            }

            // kiem tra variant co ton tai va con hang khong
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(request.VarianId, ct);

            var variant = product?.ProductVariants.FirstOrDefault(v => v.VariantId == request.VarianId);

            if(variant == null || !variant.IsActive)
            {
                return Result<CartResponse>.Failure("This product is no longer available or has been discontinued");
            }

            if(variant.Quantity < request.Quantity)
            {
                return Result<CartResponse>.Failure("Insufficient stock");
            }

            var cart = await GetOrCreateCartAsync(userId, ct);
            var existing = cart.CartItems.FirstOrDefault(i => i.VariantId == request.VarianId);

            if (existing != null)
            {
                existing.Quantity += request.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    CartId = cart.CartId,
                    VariantId = request.VarianId,
                    Quantity = request.Quantity,
                    PriceAtTime = 0
                });
            }

            cart.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveChangesAsync(ct);

            var updated = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);
            return Result<CartResponse>.Success(MapToResponse(updated!));
        }

        public async Task<Result> ClearCartAsync(Guid userId, CancellationToken ct = default)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);

            if(cart == null)
            {
                return Result.Success();
            }

            cart.CartItems.Clear();
            cart.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result<CartResponse>> GetCartAsync(Guid userId, CancellationToken ct = default)
        {
            var cart = await GetOrCreateCartAsync(userId, ct);
            return Result<CartResponse>.Success(MapToResponse(cart));
        }

        public async Task<Result<CartResponse>> RemoveItemAsync(Guid userId, Guid cartItemId, CancellationToken ct = default)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);
            
            if(cart == null)
            {
                return Result<CartResponse>.Failure("Cart not found");
            }

            var item = cart.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);
            if(item == null)
            {
                return Result<CartResponse>.Failure("The product is not in the shopping cart");
            }

            cart.CartItems.Remove(item);
            cart.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveChangesAsync(ct);

            var updated = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);

            return Result<CartResponse>.Success(MapToResponse(updated!));
        }

        public async Task<Result<CartResponse>> UpdateItemAsync(Guid userId, Guid cartItemId, UpdateCartItemRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if(errors.Count > 0)
            {
                return Result<CartResponse>.ValidationFailure(errors);
            }

            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);
            if(cart == null)
            {
                return Result<CartResponse>.Failure("Cart not exists");
            }

            var item = cart.CartItems.FirstOrDefault(i => i.CartItemId == cartItemId);
            if(item == null)
            {
                return Result<CartResponse>.Failure("The product is not in the shopping cart");
            }

            // kiem tra hang ton kho
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(item.VariantId, ct);

            var variant = product?.ProductVariants.FirstOrDefault(v => v.VariantId == item.VariantId);

            if(variant == null || !variant.IsActive)
            {
                return Result<CartResponse>.Failure("The product not exists or no longer being sold");
            }

            if(variant.Quantity < request.Quantity)
            {
                return Result<CartResponse>.Failure($"Only {variant.Quantity} left in stocks");
            }

            item.Quantity = request.Quantity;
            item.PriceAtTime = variant.Price;
            cart.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Carts.Update(cart);
            await _unitOfWork.SaveChangesAsync(ct);

            var updated = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);
            return Result<CartResponse>.Success(MapToResponse(updated!));
        }

        private async Task<Cart> GetOrCreateCartAsync(Guid userId, CancellationToken ct)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId, ct);
            if (cart != null) return cart;

            cart = new Cart { UserId = userId, UpdatedAt = DateTime.UtcNow };
            await _unitOfWork.Carts.AddAsync(cart, ct);
            await _unitOfWork.SaveChangesAsync(ct);
            return cart;
        }

        private static CartResponse MapToResponse(Cart cart)
        {
            var items = cart.CartItems.Select(i =>
            {
                var variant = i.ProductVariant;
                var product = variant?.Product;
                decimal price = variant?.Price ?? i.PriceAtTime;

                var attributes = variant?.VariantAttributes?.
                                        Select(va => $"{va.AttributeValue.Attribute.AttributeName} : {va.AttributeValue.ValueName}")
                                        .ToList() ?? new List<string>();

                return new CartItemResponse
                {
                    CartItemId = i.CartItemId,
                    VariantId = i.VariantId,
                    ProductName = product?.ProductName ?? string.Empty,
                    SKU = variant?.SKU,
                    Price = price,
                    Quantity = i.Quantity,
                    SubTotal = price * i.Quantity,
                    ImageUrl = product?.ProductImages?
                        .FirstOrDefault(img => img.IsPrimary)?.ImageUrl,
                    Attributes = attributes
                };

            }).ToList();

            return new CartResponse
            {
                CartId = cart.CartId,
                items = items,
                TotalPrice = items.Sum(i => i.SubTotal)
            };
        }
    }
}
