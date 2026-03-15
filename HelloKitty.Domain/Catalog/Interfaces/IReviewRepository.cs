using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Catalog.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review, CancellationToken ct = default);
        void Remove(Review review);
        Task<Review?> GetByIdAsync(int reviewId, CancellationToken ct = default);
        Task<PagedResult<Review>> GetByProductIdAsync(
            Guid productId, int page, int pageSize, CancellationToken ct = default);
        Task<bool> HasUserReviewedAsync(
            Guid userId, Guid productId, CancellationToken ct = default);
    }
}
