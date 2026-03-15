using HelloKitty.Domain.Common;
using HelloKitty.Domain.Common.Interfaces;
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
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(User entity, CancellationToken ct = default)
        {
            await _dbContext.Users.AddAsync(entity, ct);
        }

        public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(u => u.Email == email, ct);
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .AnyAsync(predicate, ct);
        }

        public async Task<IReadOnlyList<User>> FindAsync(Expression<Func<User, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Users
            .AsNoTracking() // bo theo doi doi tuong cac bang cua EntityF (chi doc)
            .Where(predicate)
            .ToListAsync(ct);
        }

        public async Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate, ct);
        }

        [Obsolete("Method is deprecated")]
        public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .ToListAsync();
        
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email, ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.Users.FindAsync(new object[] { id }, ct);
        }

        public async Task<User?> GetByIdWithRoleAsync(Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserId == userId, ct);
        }

        public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbContext.Users.AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            // Skip: tinh so record can bo qua
            // Take: lay so record tiep theo
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<User>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public void Remove(User entity)
        {
            _dbContext.Users.Remove(entity);
        }

        public void Update(User entity)
        {
            _dbContext.Users.Update(entity);
        }
    }
}
