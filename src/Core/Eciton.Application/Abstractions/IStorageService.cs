namespace Eciton.Application.Abstractions;
public interface IStorageService
{
    Task<string> UploadFileAsync(Stream fileStream, string fileName, string folder = null);
    Task DeleteFileAsync(string publicId);
}
