using AutoMapper;
using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Auth.DTOs;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Users.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using HelloKitty.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloKitty.Domain.Users.Entities;
using HelloKitty.Domain.Users.Enums;

namespace HelloKitty.Application.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidationService _validationService;
        public AuthService(
            IUnitOfWork uow,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IConfiguration configuration,
            IValidationService validateService
            )
        {
            _configuration = configuration;
            _uow = uow;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _validationService = validateService;
        }
        public async Task<Result<AuthResponse>> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            // Buoc 1: Validate Input
            var errors = await _validationService.ValidateAsync(request, ct);
            if(errors.Count > 0)
            {
                return Result<AuthResponse>.ValidationFailure(errors);
            }

            // Buoc 2: Tim User , khong noi ro sai tai khoan hay sai mat khau
            var user = await _uow.Users.GetByEmailAsync(request.Email, ct);
            if(user is null)
            {
                return Result<AuthResponse>.Failure("Email hoac mat khau khong dung");
            }

            // Buoc 3: Kiem tra trang thai cua tai khoan
            if(user.Status is UserStatus.Banned)
            {
                return Result<AuthResponse>.Failure("Tai khoan cua nguoi dung khong con hieu luc");
            }

            if(user.Status is UserStatus.Suspended)
            {
                return Result<AuthResponse>.Failure("Tai khoan cua nguoi dung dang bi tam khoa");
            }

            if(user.Status is UserStatus.Deleted)
            {
                return Result<AuthResponse>.Failure("Email khong xac thuc hoac password");
            }

            // Buoc 4: Kiem tra Credential
            var credential = await _uow.UserCredentials.GetByUserIdAsync(user.UserId, ct);
            if(credential is null)
            {
                return Result<AuthResponse>.Failure("Email hoac mat khau khong dung");
            }

            // Buoc 5: Kiem tra failed login (Brute-Force protection)


            // Buoc 6: Verify Password
            bool isValid = _passwordHasher.Verify(request.Password, credential.PasswordHash);

            if (!isValid)
            {
                credential.FailedLoginCount++;
                _uow.UserCredentials.Update(credential);
                await _uow.SaveChangesAsync();
                return Result<AuthResponse>.Failure("Email hoac mat khau khong dung");
            }

            // Buoc 7: Reset failed count, cap nhat lai last login
            credential.FailedLoginCount = 0;
            credential.LastLoginAt = DateTime.UtcNow;
            _uow.UserCredentials.Update(credential);
            await _uow.SaveChangesAsync();

            return await GenerateAuthResponseAsync(user, ct);
        }

        public async Task<Result> LogoutAsync(string refreshToken, CancellationToken ct = default)
        {
            var token = await _uow.RefreshTokens.GetActiveTokenAsync(refreshToken, ct);

            if(token != null)
            {
                token.IsRevoked = true;
                _uow.RefreshTokens.Update(token);
                await _uow.SaveChangesAsync();
            }

            return Result.Success();
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            // Buoc 1: Tim va kiem tra token
            var token = await _uow.RefreshTokens.GetActiveTokenAsync(refreshToken, ct);
            if(token is null || !token.IsActive)
            {
                return Result<AuthResponse>.Failure("Refresh token khong hop le hoac da het han");
            }

            // Buoc 2: Revoke token cu (token rotation - moi lan refresh tao token moi)
            token.IsRevoked = true;
            _uow.RefreshTokens.Update(token);
            await _uow.SaveChangesAsync();

            // Buoc 3: Tim user 
            var user = await _uow.Users.GetByIdWithRoleAsync(token.UserId, ct);
            if(user is null)
            {
                return Result<AuthResponse>.Failure("Nguoi dung khong ton tai");
            }

            return await GenerateAuthResponseAsync(user, ct);
        }

        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            // Buoc 1: Validate input
            var errors = await _validationService.ValidateAsync(request, ct);
            if(errors.Count > 0)
            {
                return Result<AuthResponse>.ValidationFailure(errors);
            }

            // Buoc 2: Kiem tra email ton tai
            bool emailExits = await _uow.Users.EmailExistsAsync(request.Email, ct);
            if (emailExits)
            {
                return Result<AuthResponse>.Failure("Email da duoc su dung");
            }

            // Buoc 3: Tao User entity
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Gender = request.Gender,
                Status = UserStatus.Active
            };

            // Buoc 4: Tao UserCredential
            var credential = new UserCredential
            {
                UserId = user.UserId,
                PasswordHash = _passwordHasher.Hash(request.Password),
                PasswordUpdatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
            };

            user.UserCredential = credential;

            // Buoc 5: Luu DB
            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync(ct);

            // Buoc 6: Tao Token va tra ve 
            return await GenerateAuthResponseAsync(user, ct);

        }

        private async Task<Result<AuthResponse>> GenerateAuthResponseAsync(User user, CancellationToken ct = default)
        {
            // lay user kem roles
            var userWithRoles = await _uow.Users.GetByIdWithRoleAsync(user.UserId, ct);
            var roles = userWithRoles?.UserRoles?
                .Select(r => r.Role.RoleName)
                .ToList() ?? new List<string>();
            // tao access token
            string accessToken = _tokenService.GenerateAccessToken(user, roles);

            // tao refresh token
            string refreshTokenValue = _tokenService.GenerateRefreshToken();
            int expiryDays = _configuration.GetValue<int>("Jwt:RefreshTokenExpiryDays", 7);

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                UserId = user.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
                CreatedAt = DateTime.UtcNow,
            };

            await _uow.RefreshTokens.AddAsync(refreshToken);
            await _uow.SaveChangesAsync(ct);

            int accessExpiryMinutes = _configuration.GetValue<int>("Jwt:AccessTokenExpiryMinutes", 60);

            return Result<AuthResponse>.Success(
            new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(accessExpiryMinutes)
            });
        }
    }
}
