using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.MoviePersons;

public class MoviePersonUpdate
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(100)] public string FirstName { get; set; }
    [Required] [MaxLength(100)] public string LastName { get; set; }
    [Required] public DateTime BirthDate { get; set; }
    [Required] public int CountryId { get; set; }
}