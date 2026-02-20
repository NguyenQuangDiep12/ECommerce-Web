using HelloKitty.API.Domain.Users.Entities;

namespace HelloKitty.API.Domain.Carts.Entities
{
    public class Cart
    {
        public Guid CartId { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User user { get; set; } = null!;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
        public List<CartItem> cartItems { get; set; } = new List<CartItem>();
    }
}
