using HelloKitty.Application.Features.Roles.DTOs;

namespace HelloKitty.Application.Features.Roles.DTOs
{
    public class RoleResponse
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}