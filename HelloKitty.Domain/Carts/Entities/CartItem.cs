using HelloKitty.Domain.Catalog.Entities;

namespace HelloKitty.Domain.Carts.Entities
{
    public class CartItem
    {
        public Guid CartItemId { get; private set; } = Guid.NewGuid();
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public Guid VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
