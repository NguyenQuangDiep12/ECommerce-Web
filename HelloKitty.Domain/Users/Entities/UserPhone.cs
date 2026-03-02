namespace HelloKitty.Domain.Users.Entities
{
    public class UserPhone
    {
        public int PhoneId { get; private set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public bool IsVerified { get; set; }
    }
}
