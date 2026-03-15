using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Catalog.Interfaces
{
    public interface ICategoryRepository : IReadRepository<Category, Guid>, IWriteRepository<Category>
    {
        Task AddAsync(Category category, CancellationToken ct = default);
        Task<Category?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<IReadOnlyList<Category>> GetRootCategoriesAsync(CancellationToken ct = default);
        Task<bool> SlugExistsAsync(string slug, CancellationToken ct = default);
    }
}
