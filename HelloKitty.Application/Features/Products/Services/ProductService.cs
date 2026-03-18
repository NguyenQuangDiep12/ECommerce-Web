using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Products.DTOs;
using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Catalog.Enums;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;

namespace HelloKitty.Application.Features.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public ProductService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }

        public async Task<Result<ProductDetailResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(id, ct);

            if (product is null)
                return Result<ProductDetailResponse>.Failure("Product not found");

            return Result<ProductDetailResponse>.Success(MapToDetailResponse(product));
        }

        public async Task<Result<PagedResult<ProductListResponse>>> GetFilteredAsync(
            int page, int pageSize, Guid? categoryId, string? search, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Products.GetFilteredAsync(page, pageSize, categoryId, search, ct);

            var result = new PagedResult<ProductListResponse>
            {
                Items = paged.Items.Select(p => new ProductListResponse
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Status = p.ProductStatus,
                    CategoryName = p.Category.CategoryName,
                    PrimaryImageUrl = p.ProductImages.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
                    MinPrice = p.ProductVariants.Count > 0 ? p.ProductVariants.Min(v => v.Price) : 0,
                    MaxPrice = p.ProductVariants.Count > 0 ? p.ProductVariants.Max(v => v.Price) : 0
                }),
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            };

            return Result<PagedResult<ProductListResponse>>.Success(result);
        }

        public async Task<Result<ProductDetailResponse>> CreateAsync(
            CreateProductRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<ProductDetailResponse>.ValidationFailure(errors);

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, ct);
            if (category is null)
                return Result<ProductDetailResponse>.Failure("Category not found");

            var product = new Product
            {
                ProductName = request.ProductName,
                Description = request.Description,
                CategoryId = request.CategoryId,
                ProductStatus = ProductStatus.Draft
            };

            foreach (var req in request.Variants)
            {
                var variant = new ProductVariant
                {
                    ProductId = product.ProductId,
                    SKU = req.SKU,
                    Price = req.Price,
                    Quantity = req.Quantity,
                    IsActive = true
                };

                foreach (var valueId in req.AttributeValueIds)
                {
                    variant.VariantAttributes!.Add(new VariantAttribute
                    {
                        VariantId = variant.VariantId,
                        ValueId = valueId
                    });
                }

                product.ProductVariants.Add(variant);
            }

            await _unitOfWork.Products.AddAsync(product, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            var created = await _unitOfWork.Products.GetByIdWithDetailsAsync(product.ProductId, ct);

            return Result<ProductDetailResponse>.Success(MapToDetailResponse(created!));
        }


        public async Task<Result<ProductDetailResponse>> UpdateAsync(
            Guid id, UpdateProductRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<ProductDetailResponse>.ValidationFailure(errors);

            var product = await _unitOfWork.Products.GetByIdAsync(id, ct);
            if (product is null)
                return Result<ProductDetailResponse>.Failure("Product not found");

            var category = await _unitOfWork.Categories.GetByIdAsync(request.CategoryId, ct);
            if (category is null)
                return Result<ProductDetailResponse>.Failure("Category not found");

            product.ProductName = request.ProductName;
            product.Description = request.Description;
            product.CategoryId = request.CategoryId;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            var updated = await _unitOfWork.Products.GetByIdWithDetailsAsync(id, ct);

            return Result<ProductDetailResponse>.Success(MapToDetailResponse(updated!));
        }

        public async Task<Result> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, ct);
            if (product is null)
                return Result.Failure("Product not found");

            product.ProductStatus = ProductStatus.Archived;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> ChangeStatusAsync(
            Guid id, ChangeProductStatusRequest request, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id, ct);
            if (product is null)
                return Result.Failure("Product not found");

            product.ProductStatus = request.Status;
            product.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result<VariantResponse>> AddVariantAsync(
            Guid productId, CreateVariantRequest request, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(productId, ct);
            if (product is null)
                return Result<VariantResponse>.Failure("Product not found");

            var variant = new ProductVariant
            {
                ProductId = productId,
                SKU = request.SKU,
                Price = request.Price,
                Quantity = request.Quantity,
                IsActive = true
            };

            foreach (var valueId in request.AttributeValueIds)
            {
                variant.VariantAttributes!.Add(new VariantAttribute
                {
                    VariantId = variant.VariantId,
                    ValueId = valueId
                });
            }

            product.ProductVariants.Add(variant);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<VariantResponse>.Success(MapToVariantResponse(variant));
        }

        public async Task<Result<VariantResponse>> UpdateVariantAsync(
            Guid productId, Guid variantId, UpdateVariantRequest request, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(productId, ct);
            if (product is null)
                return Result<VariantResponse>.Failure("Product not found");

            var variant = product.ProductVariants.FirstOrDefault(v => v.VariantId == variantId);
            if (variant is null)
                return Result<VariantResponse>.Failure("Variant not found");

            variant.SKU = request.SKU;
            variant.Price = request.Price;
            variant.Quantity = request.Quantity;
            variant.IsActive = request.IsActive;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<VariantResponse>.Success(MapToVariantResponse(variant));
        }

        public async Task<Result> DeleteVariantAsync(
            Guid productId, Guid variantId, CancellationToken ct = default)
        {
            var product = await _unitOfWork.Products.GetByIdWithDetailsAsync(productId, ct);
            if (product is null)
                return Result.Failure("Product not found");

            var variant = product.ProductVariants.FirstOrDefault(v => v.VariantId == variantId);
            if (variant is null)
                return Result.Failure("Variant not found");

            if (product.ProductVariants.Count == 1)
                return Result.Failure("Product must have at least one variant");

            product.ProductVariants.Remove(variant);

            _unitOfWork.Products.Update(product);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        private static ProductDetailResponse MapToDetailResponse(Product p)
        {
            return new ProductDetailResponse
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Description = p.Description,
                Status = p.ProductStatus,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName,

                Images = p.ProductImages.Select(i => new ProductImageResponse
                {
                    Id = i.ImageId,
                    ImageUrl = i.ImageUrl,
                    IsPrimary = i.IsPrimary
                }).ToList(),

                Variants = p.ProductVariants.Select(MapToVariantResponse).ToList()
            };
        }

        private static VariantResponse MapToVariantResponse(ProductVariant v)
        {
            return new VariantResponse
            {
                VariantId = v.VariantId,
                SKU = v.SKU,
                Price = v.Price,
                Quantity = v.Quantity,
                IsActive = v.IsActive,

                Attributes = v.VariantAttributes?.Select(va => new VariantAttributeResponse
                {
                    AttributeName = va.AttributeValue.Attribute.AttributeName,
                    ValueName = va.AttributeValue.ValueName
                }).ToList() ?? new List<VariantAttributeResponse>()
            };
        }
    }
}