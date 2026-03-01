using HelloKitty.Domain.Users.Entities;

namespace HelloKitty.Domain.Catalog.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public decimal Rating { get; set; } = 0;
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; private set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
