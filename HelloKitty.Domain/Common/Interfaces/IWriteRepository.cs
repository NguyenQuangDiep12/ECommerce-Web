using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Common.Interfaces
{
    /// <summary>
    /// Ghi / Sua / Xoa du lieu
    /// </summary>
    public interface IWriteRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity, CancellationToken ct = default);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
