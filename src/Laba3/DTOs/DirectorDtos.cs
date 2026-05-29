namespace MovieCatalogApi_laba3.DTOs;

public record DirectorCreateDto(
    string Name,
    string? Country
);

public record DirectorUpdateDto(
    string? Name,
    string? Country
);

public record DirectorReadDto(
    int Id,
    string Name,
    string? Country,
    int MoviesCount
);

