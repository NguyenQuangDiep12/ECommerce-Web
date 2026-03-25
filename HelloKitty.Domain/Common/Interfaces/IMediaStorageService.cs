namespace HelloKitty.Domain.Common.Interfaces
{
    public interface IMediaStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType, string folder, CancellationToken ct = default);
        Task DeleteAsync(string publicId, CancellationToken ct = default);
    }
}