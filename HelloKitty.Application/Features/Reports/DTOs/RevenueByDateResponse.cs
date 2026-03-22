namespace HelloKitty.Application.Features.Reports.DTOs
{
    public class RevenueByDateResponse
    {
        public DateTime Date { get; set; }
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}