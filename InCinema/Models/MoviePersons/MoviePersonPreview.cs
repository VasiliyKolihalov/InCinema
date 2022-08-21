using InCinema.Models.Countries;

namespace InCinema.Models.MoviePersons;

public class MoviePersonPreview
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public CountryView Country { get; set; }
}