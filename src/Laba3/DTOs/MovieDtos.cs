namespace MovieCatalogApi_laba3.DTOs;

public record MovieCreateDto(
    string Title,
    int Year,
    int DirectorId
);

public record MovieUpdateDto(
    string? Title,
    int? Year,
    int? DirectorId
);

public record MovieReadDto(
    int Id,
    string Title,
    int Year,
    string DirectorName,
    List<string> Genres
);