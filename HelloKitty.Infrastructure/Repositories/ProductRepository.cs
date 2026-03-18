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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(Product product, CancellationToken ct = default)
        {
            await _dbContext.Products.AddAsync(product);
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Products.FindAsync(new object[] { id }, ct);
        }

        public async Task<Product?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Products
                                .Include(p => p.Category)
                                .Include(p => p.ProductImages)
                                .Include(p => p.ProductVariants)
                                    .ThenInclude(v => v.VariantAttributes!)
                                        .ThenInclude(va => va.AttributeValue)
                                            .ThenInclude(av => av.Attribute)
                                .FirstOrDefaultAsync(p => p.ProductId == id , ct);
        }

        public async Task<PagedResult<Product>> GetFilteredAsync(int page, int pageSize, Guid? categoryId, string? search, CancellationToken ct = default)
        {
            var query = _dbContext.Products
                            .Include(p => p.Category)
                            .Include(p => p.ProductImages)
                            .Include(p => p.ProductVariants)
                            .AsQueryable();
            if(categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.ProductName.Contains(search));
            }

            var total = await query.CountAsync(ct);
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return new PagedResult<Product> { Items = items, TotalCount = total, Page = page, PageSize = pageSize };

        }

        public void Remove(Product entity)
        {
            _dbContext.Products.Remove(entity);
        }

        public void Update(Product entity)
        {
            _dbContext.Products.Update(entity);
        }
    }
}
