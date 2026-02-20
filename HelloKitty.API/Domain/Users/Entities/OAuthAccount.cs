using HelloKitty.API.Domain.Users.Enums;

namespace HelloKitty.API.Domain.Users.Entities
{
    public class OAuthAccount
    {
        public Guid OAuthAccountId { get; set; }
        public AuthProvider authProvider { get; set; }
        public string ProviderUserId { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public User user { get; set; } = null!;
    }
}
