using HelloKitty.Application.Features.Carts.DTOs;
using HelloKitty.Application.Features.Carts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/cart")]
    [Authorize]
    public class CartsController: BaseController
    {
        private readonly ICartService _cartService;
        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCart(CancellationToken ct)
        {
            var result = await _cartService.GetCartAsync(CurrentUserId, ct);
            return Ok(result.Value);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequest request, CancellationToken ct)
        {
            var result = await _cartService.AddItemAsync(CurrentUserId, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpPut("items/{cartItemId:guid}")]
        public async Task<IActionResult> UpdateItem(Guid cartItemId, [FromBody] UpdateCartItemRequest request, CancellationToken ct)
        {
            var result = await _cartService.UpdateItemAsync(CurrentUserId, cartItemId, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpDelete("items/{cartItemId:guid}")]
        public async Task<IActionResult> RemoveItem(Guid cartItemId, CancellationToken ct)
        {
            var result = await _cartService.RemoveItemAsync(CurrentUserId, cartItemId, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart(CancellationToken ct)
        {
            var result = await _cartService.ClearCartAsync(CurrentUserId, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }
    }
}