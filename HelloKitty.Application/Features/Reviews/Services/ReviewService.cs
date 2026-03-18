using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Reviews.DTOs;
using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Reviews.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidationService _validationService;

        public ReviewService(IValidationService validationService, IUnitOfWork unitOfWork)
        {
            _validationService = validationService;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<PagedResult<ReviewResponse>>> GetByProductIdAsync(
        Guid productId, int page, int pageSize, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Reviews.GetByProductIdAsync(productId, page, pageSize, ct);

            return Result<PagedResult<ReviewResponse>>.Success(new PagedResult<ReviewResponse>
            {
                Items = paged.Items.Select(r => new ReviewResponse {
                    ReviewId = r.ReviewId, 
                    Rating = r.Rating, 
                    Comment = r.Comment, 
                    UserName = r.User.FullName, 
                    CreatedAt = r.CreatedAt }),
                TotalCount = paged.TotalCount,
                Page = paged.Page,
                PageSize = paged.PageSize
            });
        }

        public async Task<Result<ReviewResponse>> CreateAsync(
            Guid userId, CreateReviewRequest request, CancellationToken ct = default)
        {
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<ReviewResponse>.ValidationFailure(errors);

            if (await _unitOfWork.Reviews.HasUserReviewedAsync(userId, request.ProductId, ct))
                return Result<ReviewResponse>.Failure("You have already rated this product");

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, ct);
            if (product is null)
                return Result<ReviewResponse>.Failure("Product Not Found");

            var review = new Review
            {
                UserId = userId,
                ProductId = request.ProductId,
                Rating = request.Ratings,
                Comment = request.Comments
            };

            await _unitOfWork.Reviews.AddAsync(review, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            var user = await _unitOfWork.Users.GetByIdAsync(userId, ct);

            return Result<ReviewResponse>.Success(new ReviewResponse {
                ReviewId = review.ReviewId, 
                Rating = review.Rating, 
                Comment = review.Comment,
                UserName = user?.FullName ?? string.Empty, 
                CreatedAt = review.CreatedAt});
        }

        public async Task<Result> DeleteAsync(Guid userId, int reviewId, CancellationToken ct = default)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(reviewId, ct);
            if (review is null)
                return Result.Failure("Rating does not exist");

            if (review.UserId != userId)
                return Result.Failure("You do not have permission to delete this review");

            _unitOfWork.Reviews.Remove(review);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
