namespace HelloKitty.Domain.Orders.Enums
{
    public enum ShipmentStatus
    {
        Pending = 1,          // Đã tạo shipment nhưng chưa gửi cho đơn vị vận chuyển
        Processing = 2,       // Đang chuẩn bị hàng
        Shipped = 3,          // Đã bàn giao cho đơn vị vận chuyển
        InTransit = 4,        // Đang giao
        Delivered = 5,        // Giao thành công
        Failed = 6,           // Giao thất bại
        Cancelled = 7,        // Hủy shipment
        Returned = 8          // Hàng bị trả lại
    }
}
