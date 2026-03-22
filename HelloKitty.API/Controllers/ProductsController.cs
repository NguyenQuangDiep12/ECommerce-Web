using HelloKitty.Application.Features.Products.DTOs;
using HelloKitty.Application.Features.Products.Services;
using HelloKitty.Application.Features.Reviews.DTOs;
using HelloKitty.Application.Features.Reviews.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/products")]
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IReviewService _reviewService;
        public ProductsController(
        IProductService productService,
        IReviewService reviewService)
        {
            _productService = productService;
            _reviewService = reviewService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12,
            [FromQuery] Guid? categoryId = null,
            [FromQuery] string? search = null,
            CancellationToken ct = default)
        {
            var result = await _productService.GetFilteredAsync(page, pageSize, categoryId, search, ct);
            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _productService.GetByIdAsync(id, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(new { message = result.Error });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var result = await _productService.CreateAsync(request, ct);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.Value!.ProductId }, result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken ct)
        {
            var result = await _productService.UpdateAsync(id, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpPatch("{id:guid}/status")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeProductStatusRequest request, CancellationToken ct)
        {
            var result = await _productService.ChangeStatusAsync(id, request, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var result = await _productService.DeleteAsync(id, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }

        [HttpPost("{id:guid}/variants")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> AddVariant(Guid id, [FromBody] CreateVariantRequest request, CancellationToken ct)
        {
            var result = await _productService.AddVariantAsync(id, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }

        [HttpPut("{id:guid}/variants/{variantId:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> UpdateVariant(Guid id, Guid variantId, [FromBody] UpdateVariantRequest request, CancellationToken ct)
        {
            var result = await _productService.UpdateVariantAsync(id, variantId, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }

        [HttpDelete("{id:guid}/variants/{variantId:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> DeleteVariant(Guid id, Guid variantId, CancellationToken ct)
        {
            var result = await _productService.DeleteVariantAsync(id, variantId, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }

        [HttpGet("{id:guid}/reviews")]
        public async Task<IActionResult> GetReviews(
            Guid id,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var result = await _reviewService.GetByProductIdAsync(id, page, pageSize, ct);
            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/reviews")]
        [Authorize]
        public async Task<IActionResult> CreateReview(Guid id, [FromBody] CreateReviewRequest request, CancellationToken ct)
        {
            // Gán ProductId từ route vào request
            request.ProductId = id;

            var result = await _reviewService.CreateAsync(CurrentUserId, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }
    }
}