using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace InCinema.Models.Movies;

public class MovieUpdate
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; }
    [Required] [MaxLength(500)] public string Description { get; set; }
    [Required] public DateTime ReleaseDate { get; set; }
    [Required] [Min(0)] public decimal Budget { get; set; }
    [Required] public TimeSpan Duration { get; set; }
}