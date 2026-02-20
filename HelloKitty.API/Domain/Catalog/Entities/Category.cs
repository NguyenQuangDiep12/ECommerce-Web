namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; } = Guid.NewGuid();
        public string CategoryName { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
        public List<Category> Children { get; set; } = new List<Category>();
        public string Slug { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public List<Product> products { get; set; } = new List<Product>();
    }
}
