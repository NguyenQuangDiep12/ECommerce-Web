using HelloKitty.Domain.Users.Entities;

namespace HelloKitty.Domain.Carts.Entities
{
    public class Cart
    {
        public Guid CartId { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
