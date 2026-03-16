using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Orders.DTOs;
using HelloKitty.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Orders.Services
{
    public interface IOrderService
    {
        Task<Result<OrderDetailResponse>> CreateFromCartAsync(Guid cartId, CreateOrderRequest orderRequest,CancellationToken ct = default);
        Task<Result<OrderDetailResponse>> GetByIdAsync(Guid orderId, Guid userId , CancellationToken ct = default);
        Task<Result<PagedResult<OrderListResponse>>> GetUserOrdersAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
        Task<Result> CancelOrderAsync(Guid orderId, Guid userId, CancellationToken ct = default);
        Task<Result> UpdateStatusAsync(Guid orderId , UpdateOrderStatusRequest statusRequest, CancellationToken ct = default);
    }
}
