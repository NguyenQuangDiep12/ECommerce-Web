using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HelloKitty.Application.Common;
using HelloKitty.Domain.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace HelloKitty.Infrastructure.Services
{
    public class CloudinaryService : IMediaStorageService
    {
        private readonly ICloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinarySettings> settings)
        {
            var acc = new Account(
                settings.Value.CloudName,
                settings.Value.ApiKey,
                settings.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(acc);
        }

        public async Task DeleteAsync(string publicId, CancellationToken ct = default)
        {
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            if (result.Result != "ok")
                throw new Exception($"Failed to delete image {publicId}");
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder, CancellationToken ct = default)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, fileStream),
                Folder = folder
            };
            var result = await _cloudinary.UploadAsync(uploadParams, ct);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Failed to upload image");

            return result.SecureUrl.AbsoluteUri;
        }
    }
}