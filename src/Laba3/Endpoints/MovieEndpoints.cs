using Microsoft.EntityFrameworkCore;
using MovieCatalogApi_laba3.Data;
using MovieCatalogApi_laba3.DTOs;
using MovieCatalogApi_laba3.Mappers;
using MovieCatalogApi_laba3.Models;

namespace MovieCatalogApi_laba3.Endpoints;

public static class MovieEndpoints
{
    public static void MapMovieEndpoints(this WebApplication app)
    {
        app.MapGet("/api/movies", async (AppDbContext db) =>
        {
            var movies = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .ToListAsync();

            var result = movies
                .Select(MovieMapper.ToDto)
                .ToList();

            return Results.Ok(result);
        });

        app.MapGet("/api/movies/{id:int}", async (
            int id,
            AppDbContext db
        ) =>
        {
            var movie = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
            {
                return Results.NotFound("Фильм не найден");
            }

            return Results.Ok(MovieMapper.ToDto(movie));
        });

        app.MapPost("/api/movies", async (
            MovieCreateDto dto,
            AppDbContext db
        ) =>
        {
            var directorExists = await db.Directors
                .AnyAsync(d => d.Id == dto.DirectorId);

            if (!directorExists)
            {
                return Results.BadRequest("Указанный режиссёр не существует");
            }

            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                DirectorId = dto.DirectorId
            };

            db.Movies.Add(movie);
            await db.SaveChangesAsync();

            var createdMovie = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .FirstAsync(m => m.Id == movie.Id);

            return Results.Created(
                $"/api/movies/{movie.Id}",
                MovieMapper.ToDto(createdMovie)
            );
        });

        app.MapPut("/api/movies/{id:int}", async (
            int id,
            MovieUpdateDto dto,
            AppDbContext db
        ) =>
        {
            var movie = await db.Movies.FindAsync(id);

            if (movie is null)
            {
                return Results.NotFound("Фильм не найден");
            }

            if (dto.Title is not null)
            {
                movie.Title = dto.Title;
            }

            if (dto.Year is not null)
            {
                movie.Year = dto.Year.Value;
            }

            if (dto.DirectorId is not null)
            {
                var directorExists = await db.Directors
                    .AnyAsync(d => d.Id == dto.DirectorId.Value);

                if (!directorExists)
                {
                    return Results.BadRequest("Указанный режиссёр не существует");
                }

                movie.DirectorId = dto.DirectorId.Value;
            }

            movie.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            var updatedMovie = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .FirstAsync(m => m.Id == movie.Id);

            return Results.Ok(MovieMapper.ToDto(updatedMovie));
        });

        app.MapDelete("/api/movies/{id:int}", async (
            int id,
            AppDbContext db
        ) =>
        {
            var movie = await db.Movies.FindAsync(id);

            if (movie is null)
            {
                return Results.NotFound("Фильм не найден");
            }

            db.Movies.Remove(movie);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });

        app.MapPost("/api/movies/{movieId:int}/genres/{genreId:int}", async (
            int movieId,
            int genreId,
            AppDbContext db
        ) =>
        {
            var movie = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            var genre = await db.Genres.FindAsync(genreId);

            if (movie is null || genre is null)
            {
                return Results.NotFound("Фильм или жанр не найден");
            }

            var alreadyHasGenre = movie.Genres
                .Any(g => g.Id == genreId);

            if (!alreadyHasGenre)
            {
                movie.Genres.Add(genre);
                await db.SaveChangesAsync();
            }

            return Results.Ok(MovieMapper.ToDto(movie));
        });

        app.MapDelete("/api/movies/{movieId:int}/genres/{genreId:int}", async (
            int movieId,
            int genreId,
            AppDbContext db
        ) =>
        {
            var movie = await db.Movies
                .Include(m => m.Director)
                .Include(m => m.Genres)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie is null)
            {
                return Results.NotFound("Фильм не найден");
            }

            var genre = movie.Genres
                .FirstOrDefault(g => g.Id == genreId);

            if (genre is null)
            {
                return Results.NotFound("Такого жанра у фильма нет");
            }

            movie.Genres.Remove(genre);
            await db.SaveChangesAsync();

            return Results.Ok(MovieMapper.ToDto(movie));
        });
    }
}