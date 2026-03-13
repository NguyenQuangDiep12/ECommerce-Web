using HelloKitty.Domain.Common;
using HelloKitty.Domain.Users.Entities;
using HelloKitty.Domain.Users.Interfaces;
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
    public class UserCredentialRepository : IUserCredentialRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserCredentialRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(UserCredential entity, CancellationToken ct = default)
        {
            await _dbContext.userCredentials.AddAsync(entity, ct);
        }

        public Task<bool> ExistsAsync(Expression<Func<UserCredential, bool>> predicate, CancellationToken ct = default)
        {
            return _dbContext.userCredentials
                .AsNoTracking()
                .AnyAsync(predicate, ct);
        }

        public async Task<IReadOnlyList<UserCredential>> FindAsync(Expression<Func<UserCredential, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.userCredentials
                .AsNoTracking()
                .Where(predicate)
                .ToListAsync(ct);
        }

        public Task<UserCredential?> FirstOrDefaultAsync(Expression<Func<UserCredential, bool>> predicate, CancellationToken ct = default)
        {
            return _dbContext.userCredentials
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, ct);
        }

        [Obsolete("This method is deprecated")]
        public Task<IEnumerable<UserCredential>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserCredential?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.userCredentials
                .FindAsync(new object[] { id }, ct);
        }

        public async Task<UserCredential?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.userCredentials
                .FirstOrDefaultAsync(u => u.UserId == userId, ct);
        }

        public async Task<PagedResult<UserCredential>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbContext.userCredentials.AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<UserCredential>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public void Remove(UserCredential entity)
        {
            _dbContext.userCredentials.Remove(entity);
        }

        public void Update(UserCredential entity)
        {
            _dbContext.userCredentials.Update(entity);
        }
    }
}
