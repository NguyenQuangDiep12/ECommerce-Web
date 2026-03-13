using HelloKitty.Domain.Catalog.Entities;
using HelloKitty.Domain.Common;
using HelloKitty.Domain.Orders.Entities;
using HelloKitty.Domain.Promotions.Entities;
using HelloKitty.Domain.Users.Enums;

namespace HelloKitty.Domain.Users.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; private set; } = Guid.NewGuid();
        public string FullName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public DateOnly? BirthDay { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public string? AvatarUrl { get; set; }
        public UserCredential? UserCredential { get; set; }
        public UserSensitve? UserSensitve { get; set; }
        public List<UserAddress>? UserAddresses { get; set; } = new List<UserAddress>();
        public List<UserPhone>? UserPhones { get; set; } = new List<UserPhone>();
        public List<OAuthAccount>? OAuths { get; set; } = new List<OAuthAccount>();
        public List<UserWallet>? UserWallets { get; set; } = new List<UserWallet>();
        public List<VoucherUsage>? VoucherUsages { get; set; } = new List<VoucherUsage>();
        public List<Order>? Orders { get; set; } = new List<Order>();
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<UserRole>? UserRoles { get; set; } = new List<UserRole>();
        public List<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
