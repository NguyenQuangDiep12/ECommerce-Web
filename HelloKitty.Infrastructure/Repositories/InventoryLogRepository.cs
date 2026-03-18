using HelloKitty.Domain.Inventory.Entities;
using HelloKitty.Domain.Inventory.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class InventoryLogRepository : IInventoryLogRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public InventoryLogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(InventoryLog entity, CancellationToken ct = default)
        {
            await _dbContext.InventoryLogs.AddAsync(entity, ct);
        }

        public async Task AddRangeAsync(IEnumerable<InventoryLog> entities, CancellationToken ct = default)
        {
            await _dbContext.InventoryLogs.AddRangeAsync(entities, ct);
        }

        public async Task<IEnumerable<InventoryLog>> GetByVariantIdAsync(Guid variantId, CancellationToken ct = default)
        {
            return await _dbContext.InventoryLogs
                                .AsNoTracking()
                                .Where(l => l.VariantId == variantId)
                                .OrderByDescending(l => l.CreatedAt)
                                .ToListAsync(ct);
        }
    }
}
