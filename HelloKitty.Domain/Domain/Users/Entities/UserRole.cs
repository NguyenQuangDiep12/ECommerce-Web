namespace HelloKitty.API.Domain.Users.Entities
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public DateTime AssignedAt { get; private set; }
        public Guid? AssignedBy { get; set; } // userid cua admin thuc hien gan role 
    }
}
