using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Products.DTOs;
using HelloKitty.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Products.Services
{
    public interface IProductService
    {
        Task<Result<PagedResult<ProductListResponse>>> GetFilteredAsync(
        int page, int pageSize, Guid? categoryId, string? search, CancellationToken ct = default);
        Task<Result<ProductDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<ProductDetailResponse>> CreateAsync(
            CreateProductRequest request, CancellationToken ct = default);
        Task<Result<ProductDetailResponse>> UpdateAsync(
            Guid id, UpdateProductRequest request, CancellationToken ct = default);
        Task<Result> ChangeStatusAsync(
            Guid id, ChangeProductStatusRequest request, CancellationToken ct = default);
        Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<Result<VariantResponse>> AddVariantAsync(
            Guid productId, CreateVariantRequest request, CancellationToken ct = default);
        Task<Result<VariantResponse>> UpdateVariantAsync(
            Guid productId, Guid variantId, UpdateVariantRequest request, CancellationToken ct = default);
        Task<Result> DeleteVariantAsync(
            Guid productId, Guid variantId, CancellationToken ct = default);
    }
}
