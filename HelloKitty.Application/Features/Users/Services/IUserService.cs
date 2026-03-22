using HelloKitty.Application.Common;
using HelloKitty.Application.Features.Users.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloKitty.Application.Features.Users.Services
{
    public interface IUserService
    {
        Task<Result<UserProfileResponse>> GetByIdAsync(Guid userId, CancellationToken ct = default);
        Task<Result<UserProfileResponse>> UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default);
        Task<Result<UserProfileResponse>> UpdateAvatarAsync(
            Guid userId, Stream fileStream, string fileName, string contentType, long length, CancellationToken ct = default);
        Task<Result<IReadOnlyList<AddressResponse>>> GetAddressesAsync(Guid userId, CancellationToken ct = default);
        Task<Result<AddressResponse>> AddAddressAsync(Guid userId, CreateAddressRequest request, CancellationToken ct = default);
        Task<Result<AddressResponse>> UpdateAddressAsync(Guid userId, int addressId, UpdateAddressRequest request, CancellationToken ct = default);
        Task<Result> DeleteAddressAsync(Guid userId, int addressId, CancellationToken ct = default);
    }
}
