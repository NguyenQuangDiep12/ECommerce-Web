using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Users.Interfaces
{
    public interface IUserCredentialRepository : IReadRepository<UserCredential, Guid>, IWriteRepository<UserCredential>
    {
        Task<UserCredential?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    }
}
