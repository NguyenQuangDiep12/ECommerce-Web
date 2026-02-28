using HelloKitty.API.Domain.Orders.Enums;

namespace HelloKitty.API.Domain.Orders.Entities
{
    public class Refund
    {
        public Guid RefundId { get; private set; } = Guid.NewGuid();
        public Guid PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;
        public Decimal Amount { get; set; }
        public string? Reason { get; set; }
        public RefundStatus RefundStatus { get; set; }
        public DateTime CreatedAt { get; private set; }
    }
}
