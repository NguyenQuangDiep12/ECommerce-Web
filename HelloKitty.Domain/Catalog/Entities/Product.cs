using HelloKitty.Domain.Catalog.Enums;

namespace HelloKitty.Domain.Catalog.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public required string ProductName { get; set; }
        public string? Description { get; set; }
        public ProductStatus ProductStatus { get; set; } = ProductStatus.Draft;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public List<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
