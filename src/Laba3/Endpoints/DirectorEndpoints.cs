using Microsoft.EntityFrameworkCore;
using MovieCatalogApi_laba3.Data;
using MovieCatalogApi_laba3.DTOs;
using MovieCatalogApi_laba3.Models;

namespace MovieCatalogApi_laba3.Endpoints;

public static class DirectorEndpoints
{
    public static void MapDirectorEndpoints(this WebApplication app)
    {
        app.MapGet("/api/directors", async (AppDbContext db) =>
        {
            var directors = await db.Directors
                .Select(d => new DirectorReadDto(
                    d.Id,
                    d.Name,
                    d.Country,
                    d.Movies.Count
                ))
                .ToListAsync();

            return Results.Ok(directors);
        });

        app.MapGet("/api/directors/{id:int}", async (int id, AppDbContext db) =>
        {
            var director = await db.Directors
                .Where(d => d.Id == id)
                .Select(d => new DirectorReadDto(
                    d.Id,
                    d.Name,
                    d.Country,
                    d.Movies.Count
                ))
                .FirstOrDefaultAsync();

            if (director is null)
            {
                return Results.NotFound("Режиссёр не найден");
            }

            return Results.Ok(director);
        });

        app.MapPost("/api/directors", async (
            DirectorCreateDto dto,
            AppDbContext db
        ) =>
        {
            var director = new Director
            {
                Name = dto.Name,
                Country = dto.Country
            };

            db.Directors.Add(director);
            await db.SaveChangesAsync();

            var result = new DirectorReadDto(
                director.Id,
                director.Name,
                director.Country,
                0
            );

            return Results.Created($"/api/directors/{director.Id}", result);
        });

        app.MapPut("/api/directors/{id:int}", async (
            int id,
            DirectorUpdateDto dto,
            AppDbContext db
        ) =>
        {
            var director = await db.Directors.FindAsync(id);

            if (director is null)
            {
                return Results.NotFound("Режиссёр не найден");
            }

            if (dto.Name is not null)
            {
                director.Name = dto.Name;
            }

            if (dto.Country is not null)
            {
                director.Country = dto.Country;
            }

            director.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            var result = new DirectorReadDto(
                director.Id,
                director.Name,
                director.Country,
                director.Movies.Count
            );

            return Results.Ok(result);
        });

        app.MapDelete("/api/directors/{id:int}", async (
            int id,
            AppDbContext db
        ) =>
        {
            var director = await db.Directors.FindAsync(id);

            if (director is null)
            {
                return Results.NotFound("Режиссёр не найден");
            }

            db.Directors.Remove(director);
            await db.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
