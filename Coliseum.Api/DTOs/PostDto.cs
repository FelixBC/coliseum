namespace Coliseum.Api.DTOs;

public class PostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = new();
    public Guid HostId { get; set; }
    public DateTime PostedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public UserDto Host { get; set; } = null!;
}
