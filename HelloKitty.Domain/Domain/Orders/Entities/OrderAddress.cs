namespace HelloKitty.API.Domain.Orders.Entities
{
    public class OrderAddress
    {
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public string ReceiverName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string Ward { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
    }
}
