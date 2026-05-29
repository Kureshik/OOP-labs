using MovieCatalogApi_laba3.DTOs;
using MovieCatalogApi_laba3.Models;

namespace MovieCatalogApi_laba3.Mappers;

public static class MovieMapper
{
    public static MovieReadDto ToDto(Movie movie)
    {
        return new MovieReadDto(
            movie.Id,
            movie.Title,
            movie.Year,
            movie.Director.Name,
            movie.Genres.Select(g => g.Name).ToList()
        );
    }
}