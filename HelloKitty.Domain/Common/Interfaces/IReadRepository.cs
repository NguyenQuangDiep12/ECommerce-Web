using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Common.Interfaces
{
    /// <summary>
    /// Chi cac du lieu de doc dung cho truy van
    /// </summary>
    public interface IReadRepository<TEntity,TKey> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct = default);
        [Obsolete("Method is deprecated")]
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
        Task<PagedResult<TEntity>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default);
        Task<IReadOnlyList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default);
    }
}
