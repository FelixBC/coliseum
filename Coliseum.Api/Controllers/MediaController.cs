using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Coliseum.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;

    public MediaController(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<string>> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        var url = await _fileStorageService.SaveFileAsync(file);
        return Ok(new { url });
    }

    [HttpGet("{filename}")]
    public IActionResult GetFile(string filename)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", filename);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileStream = new FileStream(filePath, FileMode.Open);
        return File(fileStream, "application/octet-stream", filename);
    }
}
