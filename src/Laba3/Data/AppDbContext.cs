using Microsoft.EntityFrameworkCore;
using MovieCatalogApi_laba3.Models;

namespace MovieCatalogApi_laba3.Data;

public class AppDbContext : DbContext
{
    public DbSet<Director> Directors => Set<Director>();

    public DbSet<Movie> Movies => Set<Movie>();

    public DbSet<Genre> Genres => Set<Genre>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Director>()
            .HasMany(d => d.Movies)
            .WithOne(m => m.Director)
            .HasForeignKey(m => m.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity<Dictionary<string, object>>(
                "MovieGenres",
                j => j.HasOne<Genre>()
                    .WithMany()
                    .HasForeignKey("GenreId"),
                j => j.HasOne<Movie>()
                    .WithMany()
                    .HasForeignKey("MovieId"),
                j =>
                {
                    j.HasKey("MovieId", "GenreId");
                    j.ToTable("MovieGenres");
                }
            );

        modelBuilder.Entity<Genre>()
            .HasIndex(g => g.Name)
            .IsUnique();
    }
}