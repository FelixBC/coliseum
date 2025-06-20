using System;
using System.Collections.Generic;

namespace Coliseum.App.Models;

public class Post
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Uri> ImageUrls { get; set; } = new();
    public Guid HostId { get; set; }
    public DateTime PostedAt { get; set; }
    public List<string> Tags { get; set; } = new();
    public User Host { get; set; } = null!;
}
