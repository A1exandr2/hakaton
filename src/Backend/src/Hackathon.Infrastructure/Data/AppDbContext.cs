using Microsoft.EntityFrameworkCore;
using Hackathon.Domain.Entities;

namespace Hackathon.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    public DbSet<Server> Servers => Set<Server>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

                entity.Property(u => u.IsDevOps);

                entity.HasIndex(u => u.Email)
                .IsUnique();
            }
        );

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Ip)
            .HasMaxLength(255).IsRequired();

            entity.Property(u => u.Name)
            .HasMaxLength(350)
            .IsRequired();

            entity.HasIndex(u => u.Name)
            .IsUnique();
        }
        );

    }
}