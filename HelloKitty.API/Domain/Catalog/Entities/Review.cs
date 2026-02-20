using HelloKitty.API.Domain.Users.Entities;

namespace HelloKitty.API.Domain.Catalog.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public decimal Rating { get; set; } = 0;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Guid UserId { get; set; }
        public User user { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product product { get; set; } = null!;
    }
}
