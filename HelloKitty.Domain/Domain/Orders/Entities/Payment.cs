using HelloKitty.API.Domain.Orders.Enums;

namespace HelloKitty.API.Domain.Orders.Entities
{
    public class Payment
    {
        public Guid PaymentId { get; set; } = Guid.NewGuid();
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; private set; }
        public Guid TransactionRef { get; set; }
        public string? FailureReason { get; set; }
        public string? GatewayResponse { get; private set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}
