using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Categories.DTOs;
using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Categories.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;
        public CategoryService(IUnitOfWork unitOfWork, IValidationService validationService)
        {
            _unitOfWork = unitOfWork;
            _validationService = validationService;
        }
        public async Task<Result<CategoryResponse>> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
            {
                return Result<CategoryResponse>.ValidationFailure(errors);
            }

            if (await _unitOfWork.Categories.SlugExistsAsync(request.Slug, ct))
            {
                return Result<CategoryResponse>.Failure("Slug already exists.");
            }

            if (request.ParentId != null)
            {
                var parent = await _unitOfWork.Categories.GetByIdAsync(request.ParentId.Value, ct);
                if (parent == null)
                {
                    return Result<CategoryResponse>.Failure("Parent category not found.");
                }
            }

            var category = new Category()
            {
                CategoryName = request.CategoryName,
                Slug = request.Slug,
                ParentId = request.ParentId,
            };

            await _unitOfWork.Categories.AddAsync(category, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<CategoryResponse>.Success(MapToResponse(category));
        }

        public async Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id, ct);

            return category is null
                ? Result<CategoryResponse>.Failure("Danh mục không tồn tại")
                : Result<CategoryResponse>.Success(MapToResponse(category));
        }

        public async Task<Result<PagedResult<CategoryResponse>>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Categories.GetPagedAsync(page, pageSize, ct);

            var items = paged.Items
                .Where(c => !c.IsDeleted)
                .Select(MapToResponse)
                .ToList();

            var result = new PagedResult<CategoryResponse>
            {
                Items = items,
                TotalCount = paged.TotalCount,
                Page = page,
                PageSize = pageSize
            };

            return Result<PagedResult<CategoryResponse>>.Success(result);
        }

        public async Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct = default)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id, ct);

            if (category == null || category.IsDeleted)
                return Result.Failure("Category not found");

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result<CategoryResponse>> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<CategoryResponse>.ValidationFailure(errors);

            var category = await _unitOfWork.Categories.GetByIdAsync(id, ct);
            if (category == null)
                return Result<CategoryResponse>.Failure("Category not found");

            if (category.Slug != request.Slug && await _unitOfWork.Categories.SlugExistsAsync(request.Slug, ct))
                return Result<CategoryResponse>.Failure("Slug not found");

            category.CategoryName = request.CategoryName;
            category.Slug = request.Slug;
            category.IsActive = request.IsActive;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result<CategoryResponse>.Success(MapToResponse(category));
        }
        private CategoryResponse MapToResponse(Category category)
        {
            var response = new CategoryResponse()
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Slug = category.Slug,
                ParentId = category.ParentId,
                IsActive = category.IsActive,
                children = new List<CategoryResponse>()
            };
            
            if(category.Children == null)
            {
                return response;
            }

            foreach(var child in category.Children)
            {
                response.children.Add(MapToResponse(child));
            }

            return response;
        }
    }
}
