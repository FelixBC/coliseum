using System;
using System.Collections.Generic;

namespace Coliseum.Api.Models;

public class UserEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;

    // Navigation properties
    public virtual ICollection<PostEntity> Posts { get; set; } = new List<PostEntity>();
}
