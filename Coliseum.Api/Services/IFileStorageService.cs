using Microsoft.AspNetCore.Http;

namespace Coliseum.Api.Services;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    string GetFileUrl(string fileName);
}
