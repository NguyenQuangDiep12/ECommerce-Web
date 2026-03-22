using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Reports.DTOs;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Orders.Enums;

namespace HelloKitty.Application.Features.Reports.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<RevenueReportResponse>> GetRevenueAsync(
            DateTime from, DateTime to, CancellationToken ct = default)
        {
            // Lấy tất cả đơn hàng Complete trong khoảng thời gian
            var paged = await _unitOfWork.Orders.GetAllPagedAsync(1, int.MaxValue, ct);

            var completedOrders = paged.Items
                .Where(o => o.OrderStatus == OrderStatus.Complete
                         && o.CreatedAt >= from
                         && o.CreatedAt <= to)
                .ToList();

            var totalRevenue = completedOrders.Sum(o => o.FinalAmount);
            var totalOrders = completedOrders.Count;

            // Group theo ngày
            var byDate = completedOrders
                .GroupBy(o => o.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new RevenueByDateResponse
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.FinalAmount),
                    OrderCount = g.Count()
                })
                .ToList();

            return Result<RevenueReportResponse>.Success(new RevenueReportResponse
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                ByDate = byDate
            });
        }

        public async Task<Result<OrderStatsResponse>> GetOrderStatsAsync(
            DateTime from, DateTime to, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Orders.GetAllPagedAsync(1, int.MaxValue, ct);

            var orders = paged.Items
                .Where(o => o.CreatedAt >= from && o.CreatedAt <= to)
                .ToList();

            return Result<OrderStatsResponse>.Success(new OrderStatsResponse
            {
                TotalOrders = orders.Count,
                Pending = orders.Count(o => o.OrderStatus == OrderStatus.Pending),
                Processing = orders.Count(o => o.OrderStatus == OrderStatus.Processing
                                             || o.OrderStatus == OrderStatus.Paid),
                Shipped = orders.Count(o => o.OrderStatus == OrderStatus.Shipped),
                Completed = orders.Count(o => o.OrderStatus == OrderStatus.Complete),
                Cancelled = orders.Count(o => o.OrderStatus == OrderStatus.Cancelled)
            });
        }

        public async Task<Result<IReadOnlyList<TopProductResponse>>> GetTopProductsAsync(
            int top, DateTime? from, DateTime? to, CancellationToken ct = default)
        {
            var paged = await _unitOfWork.Orders.GetAllPagedAsync(1, int.MaxValue, ct);

            var query = paged.Items
                .Where(o => o.OrderStatus == OrderStatus.Complete);

            if (from.HasValue) query = query.Where(o => o.CreatedAt >= from.Value);
            if (to.HasValue) query = query.Where(o => o.CreatedAt <= to.Value);

            var topProducts = query
                .SelectMany(o => o.OrderItems)
                .GroupBy(i => new
                {
                    i.ProductVariant.ProductId,
                    i.ProductVariant.Product.ProductName,
                    ImageUrl = i.ProductVariant.Product.ProductImages
                        .FirstOrDefault(img => img.IsPrimary)?.ImageUrl
                })
                .Select(g => new TopProductResponse
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    ImageUrl = g.Key.ImageUrl,
                    TotalSold = (int)g.Sum(i => i.Quantity),
                    TotalRevenue = g.Sum(i => i.UnitPrice * i.Quantity)
                })
                .OrderByDescending(p => p.TotalSold)
                .Take(top)
                .ToList();

            return Result<IReadOnlyList<TopProductResponse>>.Success(topProducts);
        }
    }
}