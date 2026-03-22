namespace HelloKitty.Application.Features.Reports.DTOs
{
    public class RevenueReportResponse
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public List<RevenueByDateResponse> ByDate { get; set; } = new List<RevenueByDateResponse>();
    }
}