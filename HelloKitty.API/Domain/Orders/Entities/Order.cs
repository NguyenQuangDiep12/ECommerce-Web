using HelloKitty.API.Domain.Orders.Enums;
using HelloKitty.API.Domain.Promotions.Entities;
using HelloKitty.API.Domain.Users.Entities;

namespace HelloKitty.API.Domain.Orders.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public OrderStatus orderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User user { get; set; } = null!;
        public Guid? VoucherId { get; set; }
        public VoucherUsage? voucherUsage { get; set; } = null!;
        public List<OrderItem> orderItems { get; set; } = new List<OrderItem>();
        public OrderAddress Address { get; set; } = null!;
        public List<Payment> payments { get; set; } = new List<Payment>();
        public Shipment? shipment { get; set; } = null!;
        public List<Refund> refunds { get; set; } = new List<Refund>();
    }
}
