using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Users.Interfaces
{
    public interface IUserRepository : IReadRepository<User, Guid> , IWriteRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdWithRoleAsync(Guid userId, CancellationToken ct = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);
        Task<User?> GetByIdWithAddressesAsync(Guid userId, CancellationToken ct = default);
    }
}
