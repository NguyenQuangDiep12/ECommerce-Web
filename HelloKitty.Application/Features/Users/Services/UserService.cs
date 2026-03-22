using HelloKitty.Application.Common;
using HelloKitty.Application.Common.Interfaces;
using HelloKitty.Application.Features.Users.DTOs;
using HelloKitty.Domain.Common.Interfaces;
using HelloKitty.Domain.Users.Entities;
using HelloKitty.Domain.Users.Interfaces;

namespace HelloKitty.Application.Features.Users.Services
{
    public class UserService(
        IUnitOfWork unitOfWork,
        IValidationService validationService,
        ICloudinaryService cloudinaryService) : IUserService
    {
        public async Task<Result<UserProfileResponse>> GetByIdAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await unitOfWork.Users.GetByIdAsync(userId, ct);

            if (user is null)
                return Result<UserProfileResponse>.Failure("User not found");

            return Result<UserProfileResponse>.Success(MapToResponse(user));
        }

        public async Task<Result<UserProfileResponse>> UpdateAsync(Guid userId, UpdateUserRequest request, CancellationToken ct = default)
        {
            var errors = await validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<UserProfileResponse>.ValidationFailure(errors);

            var user = await unitOfWork.Users.GetByIdAsync(userId, ct);
            if (user is null)
                return Result<UserProfileResponse>.Failure("User not found");

            user.FullName = request.FullName;
            user.Gender = request.Gender;
            user.BirthDay = request.BirthDay;
            user.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<UserProfileResponse>.Success(MapToResponse(user));
        }

        public async Task<Result<IReadOnlyList<AddressResponse>>> GetAddressesAsync(Guid userId, CancellationToken ct = default)
        {
            var user = await unitOfWork.Users.GetByIdWithAddressesAsync(userId, ct);

            if (user is null)
                return Result<IReadOnlyList<AddressResponse>>.Failure("User not found");

            var addresses = user.UserAddresses?
                .Select(MapAddressToResponse)
                .ToList() ?? new List<AddressResponse>();

            return Result<IReadOnlyList<AddressResponse>>.Success(addresses);
        }

        public async Task<Result<UserProfileResponse>> UpdateAvatarAsync(Guid userId, 
            Stream fileStream, string fileName, string contentType, long length, CancellationToken ct = default)
        {
            var user = await unitOfWork.Users.GetByIdAsync(userId, ct);

            if (user is null)
                return Result<UserProfileResponse>.Failure("User not found");

            var imageUrl = await cloudinaryService.UploadAsync(fileStream, fileName, $"avatars/{userId}", contentType, ct);

            user.AvatarUrl = imageUrl;
            user.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<UserProfileResponse>.Success(MapToResponse(user));
        }

        public async Task<Result<AddressResponse>> AddAddressAsync(Guid userId, CreateAddressRequest request, CancellationToken ct = default)
        {
            var errors = await validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<AddressResponse>.ValidationFailure(errors);

            var user = await unitOfWork.Users.GetByIdWithAddressesAsync(userId, ct);

            if (user is null)
                return Result<AddressResponse>.Failure("User not found");

            if (request.IsDefault && user.UserAddresses != null)
            {
                foreach (var addr in user.UserAddresses)
                    addr.IsDefault = false;
            }

            var address = new UserAddress
            {
                UserId = userId,
                Province = request.Province,
                District = request.District,
                Ward = request.Ward,
                Street = request.Street,
                IsDefault = request.IsDefault,
                CreatedAt = DateTime.UtcNow
            };

            user.UserAddresses ??= new List<UserAddress>();
            user.UserAddresses.Add(address);
            user.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<AddressResponse>.Success(MapAddressToResponse(address));
        }

        public async Task<Result<AddressResponse>> UpdateAddressAsync(Guid userId, int addressId, UpdateAddressRequest request, CancellationToken ct = default)
        {
            var errors = await validationService.ValidateAsync(request, ct);
            if (errors.Count > 0)
                return Result<AddressResponse>.ValidationFailure(errors);

            var user = await unitOfWork.Users.GetByIdWithAddressesAsync(userId, ct);

            if (user is null)
                return Result<AddressResponse>.Failure("User not found");

            var address = user.UserAddresses?.FirstOrDefault(a => a.AddressId == addressId);

            if (address is null)
                return Result<AddressResponse>.Failure("Address not found");

            if (request.IsDefault && user.UserAddresses != null)
            {
                foreach (var addr in user.UserAddresses.Where(a => a.AddressId != addressId))
                    addr.IsDefault = false;
            }

            address.Province = request.Province;
            address.District = request.District;
            address.Ward = request.Ward;
            address.Street = request.Street;
            address.IsDefault = request.IsDefault;

            user.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<AddressResponse>.Success(MapAddressToResponse(address));
        }

        public async Task<Result> DeleteAddressAsync(Guid userId, int addressId, CancellationToken ct = default)
        {
            var user = await unitOfWork.Users.GetByIdWithAddressesAsync(userId, ct);

            if (user is null)
                return Result.Failure("User not found");

            var address = user.UserAddresses?.FirstOrDefault(a => a.AddressId == addressId);

            if (address is null)
                return Result.Failure("Address not found");

            user.UserAddresses!.Remove(address);
            user.UpdatedAt = DateTime.UtcNow;

            unitOfWork.Users.Update(user);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        private static UserProfileResponse MapToResponse(User user) => new()
        {
            UserId = user.UserId,
            FullName = user.FullName,
            Email = user.Email,
            Gender = user.Gender,
            BirthDay = user.BirthDay,
            AvatarUrl = user.AvatarUrl
        };

        private static AddressResponse MapAddressToResponse(UserAddress a) => new()
        {
            AddressId = a.AddressId,
            Province = a.Province,
            District = a.District,
            Ward = a.Ward,
            Street = a.Street,
            IsDefault = a.IsDefault
        };
    }
}