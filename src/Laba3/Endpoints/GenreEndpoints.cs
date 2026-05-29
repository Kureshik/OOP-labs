using Microsoft.EntityFrameworkCore;
using MovieCatalogApi_laba3.Data;
using MovieCatalogApi_laba3.DTOs;
using MovieCatalogApi_laba3.Models;


namespace MovieCatalogApi_laba3.Endpoints;

public static class GenreEndpoints
{
    public static void MapGenreEndpoints(this WebApplication app)
    {
        app.MapGet("/api/genres", async (AppDbContext db) =>
        {
            var genres = await db.Genres
                .Select(g => new GenreReadDto(
                    g.Id,
                    g.Name,
                    g.Movies.Count
                ))
                .ToListAsync();

            return Results.Ok(genres);
        });

        app.MapGet("/api/genres/{id:int}", async (int id, AppDbContext db) =>
        {
            var genre = await db.Genres
                .Where(g => g.Id == id)
                .Select(g => new GenreReadDto(
                    g.Id,
                    g.Name,
                    g.Movies.Count
                ))
                .FirstOrDefaultAsync();

            if (genre is null)
            {
                return Results.NotFound("Жанр не найден");
            }

            return Results.Ok(genre);
        });

        app.MapPost("/api/genres", async (
            GenreCreateDto dto,
            AppDbContext db
        ) =>
        {
            var genre = new Genre
            {
                Name = dto.Name
            };

            db.Genres.Add(genre);
            await db.SaveChangesAsync();

            var result = new GenreReadDto(
                genre.Id,
                genre.Name,
                0
            );

            return Results.Created($"/api/genres/{genre.Id}", result);
        });

        app.MapPut("/api/genres/{id:int}", async (
            int id,
            GenreUpdateDto dto,
            AppDbContext db
        ) =>
        {
            var genre = await db.Genres.FindAsync(id);

            if (genre is null)
            {
                return Results.NotFound("Жанр не найден");
            }

            if (dto.Name is not null)
            {
                genre.Name = dto.Name;
            }

            genre.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            var moviesCount = await db.Movies
                .CountAsync(m => m.Genres.Any(g => g.Id == genre.Id));

            var result = new GenreReadDto(
                genre.Id,
                genre.Name,
                moviesCount
            );

            return Results.Ok(result);
        });

        app.MapDelete("/api/genres/{id:int}", async (
            int id,
            AppDbContext db
        ) =>
        {
            var genre = await db.Genres.FindAsync(id);

            if (genre is null)
            {
                return Results.NotFound("Жанр не найден");
            }

            db.Genres.Remove(genre);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}