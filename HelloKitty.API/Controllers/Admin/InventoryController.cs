using HelloKitty.Application.Features.Inventories.DTOs;
using HelloKitty.Application.Features.Inventories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers.Admin
{
    [Route("api/admin/inventory")]
    [Authorize(Roles = "Admin,Staff")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        // Xem lịch sử thay đổi tồn kho của 1 variant
        [HttpGet("{variantId:guid}")]
        public async Task<IActionResult> GetLogs(Guid variantId, CancellationToken ct)
        {
            var result = await _inventoryService.GetLogsAsync(variantId, ct);
            return Ok(result.Value);
        }

        // Nhập kho
        [HttpPost("import")]
        public async Task<IActionResult> Import([FromBody] ImportStockRequest request, CancellationToken ct)
        {
            var result = await _inventoryService.ImportStockAsync(request, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }
    }
}