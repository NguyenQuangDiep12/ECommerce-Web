using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Roles.DTOs;

namespace HelloKitty.Application.Features.Roles.Services
{
    public interface IRoleService
    {
        Task<Result<IReadOnlyList<RoleResponse>>> GetAllAsync(CancellationToken ct = default);
        Task<Result<UserRoleResponse>> GetUserRolesAsync(Guid userId, CancellationToken ct = default);
        Task<Result> AssignRoleAsync(Guid userId, AssignRoleRequest request, CancellationToken ct = default);
        Task<Result> RevokeRoleAsync(Guid userId, Guid roleId, CancellationToken ct = default);
    }
}