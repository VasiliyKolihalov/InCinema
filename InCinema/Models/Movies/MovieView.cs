using InCinema.Models.Countries;
using InCinema.Models.Genres;
using InCinema.Models.MoviePersons;
using InCinema.Models.Reviews;

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
    public MoviePersonPreview Director { get; set; }
    public IEnumerable<MoviePersonPreview> Actors { get; set; }
    public IEnumerable<GenreView> Genres { get; set; }
    public IEnumerable<ReviewView> Reviews { get; set; }
    public double? Score { get; set; }
}