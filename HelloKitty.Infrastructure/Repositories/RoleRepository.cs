using HelloKitty.Domain.Users.Entities;
using HelloKitty.Domain.Users.Interfaces;
using HelloKitty.Infrastructure.Persistences;
using Microsoft.EntityFrameworkCore;

namespace HelloKitty.Infrastructure.Repositories
{
    public class RoleRepository(ApplicationDbContext dbContext) : IRoleRepository
    {
        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default)
            => await dbContext.Roles
                .AsNoTracking()
                .Where(r => r.IsActive)
                .OrderBy(r => r.RoleName)
                .ToListAsync(ct);

        public async Task<Role?> GetByIdAsync(Guid roleId, CancellationToken ct = default)
            => await dbContext.Roles.FindAsync(new object[] { roleId }, ct);

        public async Task<Role?> GetByNameAsync(string roleName, CancellationToken ct = default)
            => await dbContext.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.RoleName == roleName, ct);
    }

    public class UserRoleRepository(ApplicationDbContext dbContext) : IUserRoleRepository
    {
        public async Task AddAsync(UserRole entity, CancellationToken ct = default)
            => await dbContext.UserRoles.AddAsync(entity, ct);

        public void Update(UserRole entity) => dbContext.UserRoles.Update(entity);

        public void Remove(UserRole entity) => dbContext.UserRoles.Remove(entity);
    }
}