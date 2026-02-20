using HelloKitty.API.Domain.Users.Enums;

namespace HelloKitty.API.Domain.Users.Entities
{
    public class UserWallet
    {
        public int UserWalletId { get; private set; }
        public Guid UserId { get; set; }
        public User user { get; set; } = null!;
        public WalletType walletType { get; set; }
        public string WalletAccountRef { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
        public DateTime LinkedAt { get; set; } = DateTime.UtcNow;
    }
}
