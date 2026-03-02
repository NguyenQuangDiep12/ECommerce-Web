using HelloKitty.Domain.Orders.Enums;

namespace HelloKitty.Domain.Orders.Entities
{
    public class Shipment
    {
        public Guid ShipmentId { get; set; } = Guid.NewGuid();
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public ShipmentProvider ShipmentProvider { get; set; }
        public string? TrackingCode { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
        public DateTime ShippedAt { get; private set; } // thoi diem don hang duoc gui di tu shop/kho den don vi van chuuyen
        public DateTime DeliveredAt { get; private set; } // thoi diem khach hang nhan hang

    }
}
