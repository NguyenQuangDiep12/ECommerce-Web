namespace HelloKitty.API.Domain.Users.Entities
{
    public class Role
    {
        public Guid RoleId { get; private set; } = Guid.NewGuid();
        public string RoleName { get; set; } = string.Empty;   // e.g. "Admin", "Customer", "Staff"
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; }
        public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
