using System.IO;
using Microsoft.AspNetCore.Http;

namespace Coliseum.Api.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _env;
    private readonly string _mediaPath;

    public FileStorageService(IWebHostEnvironment env)
    {
        _env = env;
        _mediaPath = Path.Combine(env.WebRootPath, "media");
        Directory.CreateDirectory(_mediaPath);
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File cannot be null or empty");

        // Generate unique filename to prevent conflicts
        var fileName = Path.GetRandomFileName();
        var fileExtension = Path.GetExtension(file.FileName);
        var uniqueFileName = fileName + fileExtension;
        var filePath = Path.Combine(_mediaPath, uniqueFileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return GetFileUrl(uniqueFileName);
    }

    public string GetFileUrl(string fileName)
    {
        return $"/media/{fileName}";
    }
}
