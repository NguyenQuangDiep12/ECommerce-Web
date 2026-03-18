using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Common;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ReviewRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Review review, CancellationToken ct = default)
        {
            await _dbContext.Reviews.AddAsync(review, ct);
        }

        public async Task<Review?> GetByIdAsync(int reviewId, CancellationToken ct = default)
        {
            return await _dbContext.Reviews.FindAsync(new object[] { reviewId }, ct);
        }

        public async Task<PagedResult<Review>> GetByProductIdAsync(Guid productId, int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbContext.Reviews.Include(r => r.User).Where(r => r.ProductId == productId);
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PagedResult<Review> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };
        }

        public async Task<bool> HasUserReviewedAsync(Guid userId, Guid productId, CancellationToken ct = default)
        {
            return await _dbContext.Reviews.AnyAsync(rev => rev.UserId == userId && rev.ProductId == productId , ct);
        }

        public void Remove(Review review)
        {
            _dbContext.Reviews.Remove(review);
        }
    }
}
