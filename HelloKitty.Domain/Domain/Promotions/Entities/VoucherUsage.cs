using HelloKitty.API.Domain.Orders.Entities;
using HelloKitty.API.Domain.Users.Entities;

namespace HelloKitty.API.Domain.Promotions.Entities
{
    public class VoucherUsage
    {
        public Guid VoucherId { get; private set; }
        public Voucher Voucher { get; set; } = null!;
        public DateTime UsedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
