using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Inventories.DTOs;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Inventory.Entities;
using HelloKitty.Domain.Inventory.Enums;

namespace HelloKitty.Application.Features.Inventories.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public InventoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<InventoryLogResponse>>> GetLogsAsync(Guid variantId, CancellationToken ct = default)
        {
            var logs = await _unitOfWork.InventoriesLogs.GetByVariantIdAsync(variantId, ct);

            return Result<IReadOnlyList<InventoryLogResponse>>.Success(
                logs.Select(l => new InventoryLogResponse
                {
                    LogId = l.LogId,
                    ChangeType = l.ChangeType,
                    QuantityChange = l.QuantityChange,
                    CurrentStock = l.CurrentStock,
                    ReferenceId = l.ReferenceId,
                    CreatedAt = l.CreatedAt
                }).ToList());
        }

        public async Task<Result> ImportStockAsync(ImportStockRequest request, CancellationToken ct = default)
        {
            if(request.Quantity <= 0)
            {
                return Result.Failure("Quantity must be greater than zero.");
            }

            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(request.VariantId, ct);
            var variant = product?.ProductVariants.FirstOrDefault(v => v.VariantId == request.VariantId);

            if(variant == null)
            {
                return Result.Failure("Product variant not found.");
            }

            variant.Quantity += request.Quantity;

            await _unitOfWork.InventoriesLogs.AddAsync(new InventoryLog
            {
                VariantId = variant.VariantId,
                ChangeType = ChangeType.Import,
                QuantityChange = request.Quantity,
                CurrentStock = variant.Quantity,
            }, ct);

            await _unitOfWork.Products.Update(product!);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
