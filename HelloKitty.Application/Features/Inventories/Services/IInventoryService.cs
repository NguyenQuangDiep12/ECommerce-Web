using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Inventories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Inventories.Services
{
    public interface IInventoryService
    {
        Task<Result<IReadOnlyList<InventoryLogResponse>>> GetLogsAsync(Guid variantId, CancellationToken ct = default);
        Task<Result> ImportStockAsync(ImportStockRequest request, CancellationToken ct = default);
    }
}
