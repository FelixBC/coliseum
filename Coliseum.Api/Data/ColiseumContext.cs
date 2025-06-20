using Microsoft.EntityFrameworkCore;
using Coliseum.Api.Models;

namespace Coliseum.Api.Data;

public class ColiseumContext : DbContext
{
    public ColiseumContext(DbContextOptions<ColiseumContext> options) : base(options)
    {
    }

    public DbSet<PostEntity> Posts { get; set; } = null!;
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<MediaEntity> Media { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<PostEntity>()
            .HasOne(p => p.Host)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.HostId);

        modelBuilder.Entity<MediaEntity>()
            .HasOne(m => m.Post)
            .WithMany(p => p.Media)
            .HasForeignKey(m => m.PostId);
    }
}
