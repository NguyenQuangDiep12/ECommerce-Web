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
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(RefreshToken entity, CancellationToken ct = default)
        {
            await _dbContext.refreshTokens.AddAsync(entity, ct);
        }

        [Obsolete("Method is deprecated")]
        public async Task<bool> ExistsAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.refreshTokens.AnyAsync(predicate, ct);
        }

        [Obsolete("Method is deprecated")]
        public async Task<IReadOnlyList<RefreshToken>> FindAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.refreshTokens
                .Where(predicate)
                .ToListAsync(ct);
        }

        public async Task<RefreshToken?> FirstOrDefaultAsync(Expression<Func<RefreshToken, bool>> predicate, CancellationToken ct = default)
        {
            return await _dbContext.refreshTokens
                            .Where(predicate)
                            .FirstOrDefaultAsync(ct);
             
        }

        public async Task<RefreshToken?> GetActiveTokenAsync(string token, CancellationToken ct = default)
        {
            return await _dbContext.refreshTokens
                            .FirstOrDefaultAsync(t => t.Token == token && !t.IsRevoked, ct);
        }

        [Obsolete("Do not use GetAllAsync for RefreshToken, Use GetByUserIdAsync instead .", true)]
        public Task<IEnumerable<RefreshToken>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _dbContext.refreshTokens
                .FirstOrDefaultAsync(x => x.TokenId == id, ct);
        }

        public async Task<PagedResult<RefreshToken>> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
        {
            var query = _dbContext.refreshTokens.AsNoTracking();

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(r => r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<RefreshToken>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        [Obsolete("Method is deprecated")]
        public void Remove(RefreshToken entity)
        {
            _dbContext.refreshTokens.Remove(entity);
        }

        public async Task RevokeAllUserTokensAsync(Guid userId, CancellationToken ct = default)
        {
            var tokens = await _dbContext.refreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync(ct);

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
            }

            _dbContext.refreshTokens.UpdateRange(tokens);
        }

        public void Update(RefreshToken entity)
        {
            _dbContext.refreshTokens.Update(entity);
        }
    }
}
