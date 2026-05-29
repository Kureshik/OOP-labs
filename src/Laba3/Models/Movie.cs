
namespace MovieCatalogApi_laba3.Models;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public int Year { get; set; }

    public int DirectorId { get; set; }

    public Director Director { get; set; } = null!;

    public List<Genre> Genres { get; set; } = new();
}
