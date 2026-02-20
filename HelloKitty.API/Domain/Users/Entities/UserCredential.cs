namespace HelloKitty.API.Domain.Users.Entities
{
    public class UserCredential
    {
        public Guid UserId { get; private set; }
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime PasswordUpdatedAt { get; set; }
        public int FailedLoginCount { get; set; } = 0;
        public User user { get; set; } = null!;
        public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
    }
}
