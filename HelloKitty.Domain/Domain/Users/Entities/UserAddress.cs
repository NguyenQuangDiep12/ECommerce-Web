namespace HelloKitty.API.Domain.Users.Entities
{
    public class UserAddress
    {
        public int AddressId { get; private set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

        public bool IsDefault { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
