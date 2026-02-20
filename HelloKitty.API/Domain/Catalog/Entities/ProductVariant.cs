using HelloKitty.API.Domain.Carts.Entities;
using HelloKitty.API.Domain.Inventory.Entities;
using HelloKitty.API.Domain.Orders.Entities;

namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class ProductVariant
    {
        public Guid VariantId { get; set; } = Guid.NewGuid();
        public Guid ProductId { get; set; }
        public Product product { get; set; } = null!;
        public string? SKU { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; } = false;
        public List<InventoryLog> inventoryLogs { get; set; } = new List<InventoryLog>();
        public List<VariantAttribute>? variantAttributes { get; set; } = new List<VariantAttribute>();
        public List<OrderItem>? orderItems { get; set; } = new List<OrderItem>();
        public List<CartItem>? cartItems { get; set; } = new List<CartItem>();
    }
}
