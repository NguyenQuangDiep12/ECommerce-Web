using HelloKitty.Application.Features.Vouchers.DTOs;
using HelloKitty.Application.Features.Vouchers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/vouchers")]
    public class VouchersController : BaseController
    {
        private readonly IVoucherService _voucherService;
        public VouchersController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }
        // User: kiểm tra voucher trước khi đặt hàng
        [HttpPost("validate")]
        [Authorize]
        public async Task<IActionResult> Validate([FromBody] ValidateVoucherRequest request, CancellationToken ct)
        {
            var result = await _voucherService.ValidateAsync(CurrentUserId, request, ct);
            return Ok(result.Value);
        }

        // Admin: tạo voucher mới
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateVoucherRequest request, CancellationToken ct)
        {
            var result = await _voucherService.CreateAsync(request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        // Admin: bật/tắt voucher
        [HttpPatch("{id:guid}/toggle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Toggle(Guid id, CancellationToken ct)
        {
            var result = await _voucherService.ToggleActiveAsync(id, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }
    }
}