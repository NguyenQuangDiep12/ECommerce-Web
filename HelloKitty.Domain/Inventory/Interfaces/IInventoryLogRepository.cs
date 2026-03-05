using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Inventory.Interfaces
{
    public interface IInventoryLogRepository : IAppendOnlyRepository<InventoryLog>
    {
        Task<IEnumerable<InventoryLog>> GetByVariantIdAsync(Guid variantId, CancellationToken ct = default);
    }
}
