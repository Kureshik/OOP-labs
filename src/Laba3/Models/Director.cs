namespace MovieCatalogApi_laba3.Models;

public class Director : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string? Country { get; set; }

    public List<Movie> Movies { get; set; } = new();
}
