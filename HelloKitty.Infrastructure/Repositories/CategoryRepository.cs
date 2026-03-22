using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Catalog.Interfaces;
using HelloKitty.Domain.Common;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Categories
                .Include(c => c.Children)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.CategoryId == id, ct);
        }

        [Obsolete("Using GetPagedAsync instead")]
        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Children)
                .AsSplitQuery()
                .ToListAsync(ct);
        }

        public async Task<PagedResult<Category>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            page = page <= 0 ? 1 : page;
            pageSize = pageSize <= 0 ? 10 : pageSize;

            var query = _dbContext.Categories.AsNoTracking();
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(c => c.CategoryName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Category>
            {
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<IReadOnlyList<Category>> FindAsync(Expression<Func<Category, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Categories.AsNoTracking().Where(predicate).ToListAsync(ct);
        }

        public async Task<Category?> FirstOrDefaultAsync(Expression<Func<Category, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Categories.AsNoTracking().FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Category, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Categories.AnyAsync(predicate, ct);
        }

        public async Task<Category?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Children)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Slug == slug, ct);
        }

        public async Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
        {
            return await _dbContext.Categories
                .Include(c => c.Children)
                .Where(c => c.ParentId == null && c.IsActive)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default)
        {
            return await _dbContext.Categories
                .AnyAsync(c => c.Slug.ToLower() == slug.ToLower(), ct);
        }

        public async Task AddAsync(Category entity, CancellationToken ct = default)
        {
            await _dbContext.Categories.AddAsync(entity, ct);
        }

        public void Update(Category entity) => _dbContext.Categories.Update(entity);

        public void Remove(Category entity) => _dbContext.Categories.Remove(entity);
    }
}
