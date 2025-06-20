using System;

namespace Coliseum.Api.Models;

public class MediaEntity
{
    public Guid Id { get; set; }
    public Guid PostId { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? Caption { get; set; }

    // Navigation properties
    public virtual PostEntity Post { get; set; } = null!;
}
