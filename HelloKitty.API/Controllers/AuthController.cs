using HelloKitty.Application.Features.Auth.DTOs;
using HelloKitty.Application.Features.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelloKitty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var result = await _authService.RegisterAsync(request, ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            if (result.IsValidationError)
            {
                return UnprocessableEntity(new
                {
                    message = result.Error,
                    errors = result.ValidationErrors
                });
            }

            return BadRequest(new
            {
                message = result.Error
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await _authService.LoginAsync(request, ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            if (result.IsValidationError)
            {
                return UnprocessableEntity(new
                {
                    message = result.Error,
                    errors = result.ValidationErrors
                });
            }

            return Unauthorized(new { message = result.Error });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken, CancellationToken ct)
        {
            var result = await _authService.RefreshTokenAsync(refreshToken, ct);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Unauthorized(new { message = result.Error });
        }

        public async Task<IActionResult> Logout([FromBody] string refreshToken, CancellationToken ct)
        {
            await _authService.LogoutAsync(refreshToken, ct);

            return Ok();
        }
    }
}
