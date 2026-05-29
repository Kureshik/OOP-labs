namespace MovieCatalogApi_laba3.DTOs;

public record GenreCreateDto(
    string Name
);

public record GenreUpdateDto(
    string? Name
);

public record GenreReadDto(
    int Id,
    string Name,
    int MoviesCount
);
