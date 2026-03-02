using HelloKitty.Domain.Catalog.Entities;

namespace HelloKitty.Domain.Carts.Entities
{
    public class CartItem
    {
        public Guid CartItemId { get; private set; } = Guid.NewGuid();
        public Guid CartId { get; private set; }
        public Cart Cart { get; set; } = null!;
        public int Quantity { get; set; }
        public DateTime PriceAtTime { get; set; }
        public Guid VariantId { get; set; }
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}
