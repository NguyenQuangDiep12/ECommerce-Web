using HelloKitty.API.Domain.Catalog.Entities;
using HelloKitty.API.Domain.Common;
using HelloKitty.API.Domain.Orders.Entities;
using HelloKitty.API.Domain.Promotions.Entities;
using HelloKitty.API.Domain.Users.Enums;

namespace HelloKitty.API.Domain.Users.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; private set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly? BirthDay { get; set; }
        public required string Email { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public string? AvatarUrl { get; set; }
        public UserCredential? userCredential { get; set; }
        public UserSensitve? userSensitve { get; set; }
        public List<OAuthAccount>? oAuths { get; set; } = new List<OAuthAccount>();
        public List<UserWallet>? userWallets { get; set; } = new List<UserWallet>();
        public List<VoucherUsage>? voucherUsages { get; set; } = new List<VoucherUsage>();
        public List<Order>? orders { get; set; } = new List<Order>();
        public List<Review>? reviews { get; set; } = new List<Review>();
    }
}
