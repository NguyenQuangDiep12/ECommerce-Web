using HelloKitty.Domain.Common;
using HelloKitty.Domain.Orders.Entities;
using HelloKitty.Domain.Orders.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public OrderRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Order order, CancellationToken ct = default)
        {
            await _dbContext.Orders.AddAsync(order, ct);
        }

        public async Task<PagedResult<Order>> GetAllPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbContext.Orders
                            .Include(o => o.OrderItems)
                            .Include(o => o.Address)
                            .Include(o => o.Payments);
            var total = await query.CountAsync();
            var items = await query
                            .OrderByDescending(o => o.CreatedAt)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .AsNoTracking()
                            .ToListAsync();
            return new PagedResult<Order> { Items = items , TotalCount = total, Page = page, PageSize = pageSize};
        }

        public async Task<Order?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Orders
                            .Include(o => o.OrderItems)
                                .ThenInclude(i => i.ProductVariant)
                                    .ThenInclude(v => v.Product)
                            .Include(o => o.Address)
                            .Include(o => o.Payments)
                            .Include(o => o.Shipment)
                            .FirstOrDefaultAsync(o => o.OrderId == id, ct);
        }

        public async Task<PagedResult<Order>> GetByUserIdPagedAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbContext.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Address)
            .Include(o => o.Payments)
            .Where(o => o.UserId == userId);

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PagedResult<Order> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
        }

        public void Remove(Order entity)
        {
             _dbContext.Orders.Remove(entity);
        }

        public void Update(Order entity)
        {
             _dbContext.Orders.Update(entity);
        }
    }
}
