namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class ProductImage
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsPrimary { get; set; } = false;
        public Guid ProductId { get; set; }
        public Product product { get; set; } = null!;
    }
}
