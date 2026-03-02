using HelloKitty.Domain.Users.Enums;

namespace HelloKitty.Domain.Users.Entities
{
    public class Permission
    {
        public Guid PermissionId { get; set; } = Guid.NewGuid();
        public string PermissionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public PermissionGroup Group { get; set; }
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
