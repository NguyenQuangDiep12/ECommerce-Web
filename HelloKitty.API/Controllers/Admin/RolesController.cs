using HelloKitty.Application.Features.Roles.DTOs;
using HelloKitty.Application.Features.Roles.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers.Admin
{
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class RolesController: ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        // Lấy danh sách tất cả roles
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles(CancellationToken ct)
        {
            var result = await _roleService.GetAllAsync(ct);
            return Ok(result.Value);
        }

        // Gán role cho user
        [HttpPost("users/{userId:guid}/roles")]
        public async Task<IActionResult> AssignRole(Guid userId, [FromBody] AssignRoleRequest request, CancellationToken ct)
        {
            var result = await _roleService.AssignRoleAsync(userId, request, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }

        // Thu hồi role của user
        [HttpDelete("users/{userId:guid}/roles/{roleId:guid}")]
        public async Task<IActionResult> RevokeRole(Guid userId, Guid roleId, CancellationToken ct)
        {
            var result = await _roleService.RevokeRoleAsync(userId, roleId, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }

        // Xem danh sách roles của 1 user
        [HttpGet("users/{userId:guid}/roles")]
        public async Task<IActionResult> GetUserRoles(Guid userId, CancellationToken ct)
        {
            var result = await _roleService.GetUserRolesAsync(userId, ct);
            return Ok(result.Value);
        }
    }
}