using HelloKitty.Domain.Orders.Enums;
using HelloKitty.Domain.Promotions.Entities;
using HelloKitty.Domain.Users.Entities;

namespace HelloKitty.Domain.Orders.Entities
{
    public class Order
    {
        public Guid OrderId { get; set; } = Guid.NewGuid();
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid? VoucherId { get; set; }
        public VoucherUsage? VoucherUsage { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public OrderAddress Address { get; set; } = null!;
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public Guid ShipmentId { get; set; } 
        public Shipment? Shipment { get; set; } = null!;
    }
}
