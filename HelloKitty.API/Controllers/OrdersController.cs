using HelloKitty.Application.Features.Orders.DTOs;
using HelloKitty.Application.Features.Orders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequest request, CancellationToken ct)
        {
            var result = await _orderService.CreateFromCartAsync(CurrentUserId, request, ct);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.Value!.OrderId }, result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var result = await _orderService.GetUserOrdersAsync(CurrentUserId, page, pageSize, ct);
            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _orderService.GetByIdAsync(id, CurrentUserId, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.Error == "Authority not permitted")
                return Forbid();

            return NotFound(new { message = result.Error });
        }

        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
        {
            var result = await _orderService.CancelOrderAsync(id, CurrentUserId, ct);

            if (result.IsSuccess)
                return NoContent();

            if (result.Error == "authority not permitted")
                return Forbid();

            return BadRequest(new { message = result.Error });
        }
    }
}