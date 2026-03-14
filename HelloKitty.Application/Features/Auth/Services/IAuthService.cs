using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Auth.Services
{
    public interface IAuthService
    {
        Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
        Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
        Task<Result> LogoutAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
