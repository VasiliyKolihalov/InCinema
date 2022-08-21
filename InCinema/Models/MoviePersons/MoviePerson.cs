using InCinema.Models.Countries;

namespace InCinema.Models.MoviePersons;

public class MoviePerson
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public DateTime BirthDate { get; set; }
    public Country Country { get; set; }
}