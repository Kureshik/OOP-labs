namespace MovieCatalogApi_laba3.Models;

public class Genre : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public List<Movie> Movies { get; set; } = new();
}
