namespace HelloKitty.Application.Features.Reports.DTOs
{
    public class OrderStatsResponse
    {
        public int TotalOrders { get; set; }
        public int Pending { get; set; }
        public int Processing { get; set; }
        public int Shipped { get; set; }
        public int Completed { get; set; }
        public int Cancelled { get; set; }
    }
}