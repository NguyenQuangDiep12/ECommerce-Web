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
            // Step 1: Validate Input
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
            {
                return Result<AuthResponse>.ValidationFailure(errors);
            }

            // Step 2: Find User (do not specify whether email or password is incorrect)
            var user = await _uow.Users.GetByEmailAsync(request.Email, ct);
            if (user is null)
            {
                return Result<AuthResponse>.Failure("Invalid email or password");
            }

            // Step 3: Check account status
            if (user.Status is UserStatus.Banned)
            {
                return Result<AuthResponse>.Failure("User account is no longer active");
            }

            if (user.Status is UserStatus.Suspended)
            {
                return Result<AuthResponse>.Failure("User account is temporarily suspended");
            }

            if (user.Status is UserStatus.Deleted)
            {
                return Result<AuthResponse>.Failure("Invalid email or password");
            }

            // Step 4: Check Credential
            var credential = await _uow.UserCredentials.GetByUserIdAsync(user.UserId, ct);
            if (credential is null)
            {
                return Result<AuthResponse>.Failure("Invalid email or password");
            }

            // Step 5: Brute-force protection (not implemented yet)

            // Step 6: Verify Password
            bool isValid = _passwordHasher.Verify(request.Password, credential.PasswordHash);

            if (!isValid)
            {
                credential.FailedLoginCount++;
                _uow.UserCredentials.Update(credential);
                await _uow.SaveChangesAsync();
                return Result<AuthResponse>.Failure("Invalid email or password");
            }

            // Step 7: Reset failed count and update last login
            credential.FailedLoginCount = 0;
            credential.LastLoginAt = DateTime.UtcNow;
            _uow.UserCredentials.Update(credential);
            await _uow.SaveChangesAsync();

            return await GenerateAuthResponseAsync(user, ct);
        }
        public async Task<Result> LogoutAsync(string refreshToken, CancellationToken ct = default)
        {
            var token = await _uow.RefreshTokens.GetActiveTokenAsync(refreshToken, ct);

            if (token != null)
            {
                token.IsRevoked = true;
                _uow.RefreshTokens.Update(token);
                await _uow.SaveChangesAsync();
            }

            return Result.Success();
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
        {
            // Step 1: Find and validate token
            var token = await _uow.RefreshTokens.GetActiveTokenAsync(refreshToken, ct);
            if (token is null || !token.IsActive)
            {
                return Result<AuthResponse>.Failure("Invalid or expired refresh token");
            }

            // Step 2: Revoke old token (token rotation)
            token.IsRevoked = true;
            _uow.RefreshTokens.Update(token);
            await _uow.SaveChangesAsync();

            // Step 3: Find user
            var user = await _uow.Users.GetByIdWithRoleAsync(token.UserId, ct);
            if (user is null)
            {
                return Result<AuthResponse>.Failure("User not found");
            }

            return await GenerateAuthResponseAsync(user, ct);
        }


        public async Task<Result<AuthResponse>> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
        {
            // Step 1: Validate input
            var errors = await _validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
            {
                return Result<AuthResponse>.ValidationFailure(errors);
            }

            // Step 2: Check if email exists
            bool emailExists = await _uow.Users.EmailExistsAsync(request.Email, ct);
            if (emailExists)
            {
                return Result<AuthResponse>.Failure("Email is already in use");
            }

            // Step 3: Create User entity
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                Gender = request.Gender,
                Status = UserStatus.Active
            };

            // Step 4: Create UserCredential
            var credential = new UserCredential
            {
                UserId = user.UserId,
                PasswordHash = _passwordHasher.Hash(request.Password),
                PasswordUpdatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
            };

            user.UserCredential = credential;

            // Step 5: Save to DB
            await _uow.Users.AddAsync(user);
            await _uow.SaveChangesAsync(ct);

            // Step 6: Generate token and return
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
