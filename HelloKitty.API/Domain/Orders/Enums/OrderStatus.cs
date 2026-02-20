namespace HelloKitty.API.Domain.Orders.Enums
{
    public enum OrderStatus
    {
        Pending = 0, 
        Paid = 1,
        Processing = 2,
        Shipped = 3,
        Complete = 4,
        Cancelled = 5,
        Refund = 6,
    }
}
