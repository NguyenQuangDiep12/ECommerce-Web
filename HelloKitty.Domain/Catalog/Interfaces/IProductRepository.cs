using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Catalog.Interfaces
{
    public interface IProductRepository : IWriteRepository<Product>
    {
        Task AddAsync(Product product, CancellationToken ct = default);
        Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Product?> GetByIdWithDetailsAsync(Guid id, CancellationToken ct = default);
        Task<PagedResult<Product>> GetFilteredAsync(
            int page, int pageSize,
            Guid? categoryId, string? search,
            CancellationToken ct = default);
    }
}
