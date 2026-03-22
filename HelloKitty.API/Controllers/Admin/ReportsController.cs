using HelloKitty.Application.Features.Reports.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers.Admin
{
    [Route("api/admin/reports")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }
        // Doanh thu theo khoảng thời gian
        [HttpGet("revenue")]
        public async Task<IActionResult> GetRevenue(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            CancellationToken ct)
        {
            var result = await _reportService.GetRevenueAsync(from, to, ct);
            return Ok(result.Value);
        }

        // Thống kê đơn hàng theo trạng thái
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrderStats(
            [FromQuery] DateTime from,
            [FromQuery] DateTime to,
            CancellationToken ct)
        {
            var result = await _reportService.GetOrderStatsAsync(from, to, ct);
            return Ok(result.Value);
        }

        // Top sản phẩm bán chạy
        [HttpGet("products")]
        public async Task<IActionResult> GetTopProducts(
            [FromQuery] int top = 10,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            CancellationToken ct = default)
        {
            var result = await _reportService.GetTopProductsAsync(top, from, to, ct);
            return Ok(result.Value);
        }
    }
}