using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Reports.DTOs;

namespace HelloKitty.Application.Features.Reports.Services
{
    public interface IReportService
    {
        Task<Result<RevenueReportResponse>> GetRevenueAsync(DateTime from, DateTime to, CancellationToken ct = default);
        Task<Result<OrderStatsResponse>> GetOrderStatsAsync(DateTime from, DateTime to, CancellationToken ct = default);
        Task<Result<IReadOnlyList<TopProductResponse>>> GetTopProductsAsync(int top, DateTime? from, DateTime? to, CancellationToken ct = default);
    }
}