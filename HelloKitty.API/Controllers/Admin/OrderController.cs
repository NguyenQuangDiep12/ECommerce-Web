using HelloKitty.Application.Features.Orders.DTOs;
using HelloKitty.Application.Features.Orders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers.Admin
{
    [Route("api/admin/orders")]
    [Authorize(Roles = "Admin,Staff")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public AdminController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        // Lấy tất cả đơn hàng (có phân trang)
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            var result = await _orderService.GetAllOrdersAsync(page, pageSize, ct);
            return Ok(result.Value);
        }

        // Xem chi tiết 1 đơn hàng bất kỳ
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _orderService.GetByIdForAdminAsync(id, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(new { message = result.Error });
        }

        // Cập nhật trạng thái đơn hàng
        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
        {
            var result = await _orderService.UpdateStatusAsync(id, request, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }

        // Tạo shipment cho đơn hàng
        [HttpPost("{id:guid}/shipment")]
        public async Task<IActionResult> CreateShipment(Guid id, [FromBody] CreateShipmentRequest request, CancellationToken ct)
        {
            var result = await _orderService.CreateShipmentAsync(id, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }
    }
}