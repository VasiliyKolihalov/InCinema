using InCinema.Models.Countries;
using InCinema.Models.Genres;

namespace InCinema.Models.Movies;

public class MovieView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Budget { get; set; }
    public TimeSpan Duration { get; set; }
    public CountryView Country { get; set; }
    public List<GenreView> Genres { get; set; }
}