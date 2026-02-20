using HelloKitty.API.Domain.Catalog.Enums;

namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public required string ProductName { get; set; }
        public string? Description { get; set; }
        public ProductStatus productStatus { get; set; } = ProductStatus.Draft;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public List<Review> reviews { get; set; } = new List<Review>();
        public Guid CategoryId { get; set; }
        public Category category { get; set; } = null!;
        public List<ProductImage> productImages { get; set; } = new List<ProductImage>();
        public List<ProductVariant> productVariants { get; set; } = new List<ProductVariant>();
    }
}
