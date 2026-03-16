using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Orders.Interfaces
{
    public interface IOrderRepository : IWriteRepository<Order>
    {
        Task AddAsync(Order order, CancellationToken ct = default);
        Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<PagedResult<Order>> GetByUserIdPagedAsync(
            Guid userId, int page, int pageSize, CancellationToken ct = default);
        Task<PagedResult<Order>> GetAllPagedAsync(
            int page, int pageSize, CancellationToken ct = default);
    }
}
