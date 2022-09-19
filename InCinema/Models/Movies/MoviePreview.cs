using InCinema.Models.Countries;

namespace InCinema.Models.Movies;

public class MoviePreview
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
    public decimal Budget { get; set; }
    public TimeSpan Duration { get; set; }
    public CountryView Country { get; set; }
    public double? Score { get; set; }
}