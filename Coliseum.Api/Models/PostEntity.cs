using System;
using System.Collections.Generic;

namespace Coliseum.Api.Models;

public class PostEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid HostId { get; set; }
    public DateTime PostedAt { get; set; }
    public string? Tags { get; set; } // Comma-separated tags

    // Navigation properties
    public virtual UserEntity Host { get; set; } = null!;
    public virtual ICollection<MediaEntity> Media { get; set; } = new List<MediaEntity>();
}
