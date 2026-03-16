using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Orders.DTOs;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IValidationService _validationService;
        public OrderService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }

        public Task<Result> CancelOrderAsync(Guid orderId, Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderDetailResponse>> CreateFromCartAsync(Guid cartId, CreateOrderRequest orderRequest, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderDetailResponse>> GetByIdAsync(Guid orderId, Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result<PagedResult<OrderListResponse>>> GetUserOrdersAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateStatusAsync(Guid orderId, UpdateOrderStatusRequest statusRequest, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
