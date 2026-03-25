using HelloKitty.Application.Features.Users.DTOs;
using HelloKitty.Application.Features.Users.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UserController: BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProfile(Guid id, CancellationToken ct)
        {
            var result = await _userService.GetByIdAsync(id, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(new { message = result.Error });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProfile(Guid id, [FromBody] UpdateUserRequest request, CancellationToken ct)
        {
            if (id != CurrentUserId)
                return Forbid();

            var result = await _userService.UpdateAsync(id, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpPost("{id:guid}/avatar")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadAvatar(Guid id, IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File is empty" });

            using var stream = file.OpenReadStream();
            

            var result = await _userService.UpdateAvatarAsync(id, stream, Path.GetFileName(file.FileName), file.ContentType, file.Length, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }

        [HttpGet("{id:guid}/addresses")]
        public async Task<IActionResult> GetAddresses(Guid id, CancellationToken ct)
        {
            if (id != CurrentUserId)
                return Forbid();

            var result = await _userService.GetAddressesAsync(id, ct);
            return Ok(result.Value);
        }

        [HttpPost("{id:guid}/addresses")]
        public async Task<IActionResult> AddAddress(Guid id, [FromBody] CreateAddressRequest request, CancellationToken ct)
        {
            if (id != CurrentUserId)
                return Forbid();

            var result = await _userService.AddAddressAsync(id, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.IsValidationError)
                return UnprocessableEntity(new { message = result.Error, errors = result.ValidationErrors });

            return BadRequest(new { message = result.Error });
        }

        [HttpPut("{id:guid}/addresses/{addressId:int}")]
        public async Task<IActionResult> UpdateAddress(Guid id, int addressId, [FromBody] UpdateAddressRequest request, CancellationToken ct)
        {
            if (id != CurrentUserId)
                return Forbid();

            var result = await _userService.UpdateAddressAsync(id, addressId, request, ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(new { message = result.Error });
        }

        [HttpDelete("{id:guid}/addresses/{addressId:int}")]
        public async Task<IActionResult> DeleteAddress(Guid id, int addressId, CancellationToken ct)
        {
            if (id != CurrentUserId)
                return Forbid();

            var result = await _userService.DeleteAddressAsync(id, addressId, ct);

            if (result.IsSuccess)
                return NoContent();

            return BadRequest(new { message = result.Error });
        }
    }
}