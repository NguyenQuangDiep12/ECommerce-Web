using HelloKitty.API.Domain.Catalog.Entities;

namespace HelloKitty.API.Domain.Orders.Entities
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public uint Quantity { get; set; } = 0;
        public decimal UnitPrice { get; set; }
        public Guid VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;

    }
}
