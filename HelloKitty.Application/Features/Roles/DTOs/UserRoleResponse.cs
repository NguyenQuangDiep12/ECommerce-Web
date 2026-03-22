namespace HelloKitty.Application.Features.Roles.DTOs
{
    public class UserRoleResponse
    {
        public Guid UserId { get; set; }
        public List<RoleResponse> Roles { get; set; } = new();
    }
}