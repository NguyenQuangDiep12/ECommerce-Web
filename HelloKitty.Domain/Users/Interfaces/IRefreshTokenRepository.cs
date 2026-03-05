using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Users.Interfaces
{
    public interface IRefreshTokenRepository : IReadRepository<RefreshToken, Guid>, IWriteRepository<RefreshToken>
    {
        Task<RefreshToken?> GetActiveTokenAsync(string token, CancellationToken ct = default);
        Task RevokeAllUserTokensAsync(Guid userId, CancellationToken ct = default);
    }
}
