using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Roles.DTOs;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Users.Entities;

namespace HelloKitty.Application.Features.Roles.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IReadOnlyList<RoleResponse>>> GetAllAsync(CancellationToken ct = default)
        {
            var roles = await _unitOfWork.Roles.GetAllAsync(ct);

            var response = roles
                .Select(r => new RoleResponse
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Description = r.Description,
                    IsActive = r.IsActive
                })
                .ToList();

            return Result<IReadOnlyList<RoleResponse>>.Success(response);
        }

        public async Task<Result<UserRoleResponse>> GetUserRolesAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId, ct);
            if (user is null)
                return Result<UserRoleResponse>.Failure("Người dùng không tồn tại");

            var roles = user.UserRoles?
                .Select(ur => new RoleResponse
                {
                    RoleId = ur.Role.RoleId,
                    RoleName = ur.Role.RoleName,
                    Description = ur.Role.Description,
                    IsActive = ur.Role.IsActive
                })
                .ToList() ?? new List<RoleResponse>();

            return Result<UserRoleResponse>.Success(new UserRoleResponse
            {
                UserId = userId,
                Roles = roles
            });
        }

        public async Task<Result> AssignRoleAsync(Guid userId, AssignRoleRequest request, CancellationToken ct = default)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId, ct);
            if (user is null)
                return Result.Failure("Người dùng không tồn tại");

            var role = await _unitOfWork.Roles.GetByIdAsync(request.RoleId, ct);
            if (role is null)
                return Result.Failure("Role không tồn tại");

            // Kiểm tra đã có role này chưa
            bool alreadyHasRole = user.UserRoles?.Any(ur => ur.RoleId == request.RoleId) ?? false;
            if (alreadyHasRole)
                return Result.Failure("Người dùng đã có role này");

            var userRole = new UserRole
            {
                UserId = userId,
                RoleId = request.RoleId
            };

            await _unitOfWork.UserRoles.AddAsync(userRole, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> RevokeRoleAsync(Guid userId, Guid roleId, CancellationToken ct = default)
        {
            var user = await _unitOfWork.Users.GetByIdWithRoleAsync(userId, ct);
            if (user is null)
                return Result.Failure("Người dùng không tồn tại");

            var userRole = user.UserRoles?.FirstOrDefault(ur => ur.RoleId == roleId);
            if (userRole is null)
                return Result.Failure("Người dùng không có role này");

            _unitOfWork.UserRoles.Remove(userRole);
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}