

namespace HelloKitty.Domain.Users.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder, CancellationToken ct = default);
        Task DeleteAsync(string publicId, CancellationToken ct = default);
    }
}