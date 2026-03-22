using HelloKitty.Application.Features.Categories.DTOs;
using HelloKitty.Application.Features.Categories.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var result = await _categoryService.GetPagedAsync(page, pageSize, ct);
            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        {
            var result = await _categoryService.GetByIdAsync(id, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(new { message = result.Error });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request, CancellationToken ct)
        {
            var result = await _categoryService.CreateAsync(request, ct);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.Value!.CategoryId }, result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request, CancellationToken ct)
        {
            var result = await _categoryService.UpdateAsync(id, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var result = await _categoryService.SoftDeleteAsync(id, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }
    }
}
