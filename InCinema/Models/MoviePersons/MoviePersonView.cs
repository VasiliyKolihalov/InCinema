using InCinema.Models.Careers;
using InCinema.Models.Countries;

namespace InCinema.Models.MoviePersons;

public class MoviePersonView
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public CountryView Country { get; set; }
    public IEnumerable<CareerView> Careers { get; set; }
}