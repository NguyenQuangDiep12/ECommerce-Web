using HelloKitty.API.Domain.Carts.Entities;
using HelloKitty.API.Domain.Inventory.Entities;
using HelloKitty.API.Domain.Orders.Entities;

namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class ProductVariant
    {
        public Guid VariantId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = false;
        public List<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
        public List<VariantAttribute>? VariantAttributes { get; set; } = new List<VariantAttribute>();
        public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
        public List<CartItem>? CartItems { get; set; } = new List<CartItem>();
    }
}
