using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Categories.DTOs;
using HelloKitty.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Categories.Services
{
    public interface ICategoryService
    {
        Task<Result<PagedResult<CategoryResponse>>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
        Task<Result<CategoryResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<CategoryResponse>> CreateAsync(CreateCategoryRequest request, CancellationToken ct = default);
        Task<Result<CategoryResponse>> UpdateAsync(Guid id, UpdateCategoryRequest request, CancellationToken ct = default);
        Task<Result> SoftDeleteAsync(Guid id, CancellationToken ct = default);
    }
}
