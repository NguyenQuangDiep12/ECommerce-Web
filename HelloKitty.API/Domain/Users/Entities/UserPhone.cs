namespace HelloKitty.API.Domain.Users.Entities
{
    public class UserPhone
    {
        public int PhoneId { get; private set; }
        public Guid UserId { get; set; }
        public User user { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
        public bool IsVerified { get; set; } = false;
    }
}
