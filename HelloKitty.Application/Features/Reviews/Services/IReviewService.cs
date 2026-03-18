using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Reviews.DTOs;
using HelloKitty.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Reviews.Services
{
    public interface IReviewService
    {
        Task<Result<PagedResult<ReviewResponse>>> GetByProductIdAsync(
        Guid productId, int page, int pageSize, CancellationToken ct = default);
        Task<Result<ReviewResponse>> CreateAsync(
            Guid userId, CreateReviewRequest request, CancellationToken ct = default);
        Task<Result> DeleteAsync(Guid userId, int reviewId, CancellationToken ct = default);
    }
}
