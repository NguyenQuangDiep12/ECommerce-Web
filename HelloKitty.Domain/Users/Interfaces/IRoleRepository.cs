using HelloKitty.Domain.Users.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Domain.Users.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default);
        Task<Role?> GetByIdAsync(Guid roleId, CancellationToken ct = default);
        Task<Role?> GetByNameAsync(string roleName, CancellationToken ct = default);
    }
}
