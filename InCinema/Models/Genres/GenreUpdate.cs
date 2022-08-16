using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace InCinema.Models.Genres;

public class GenreUpdate
{
    [Required] public int Id { get; set; }
    [Required] [MaxLength(50)] public string Name { get; set; }
    [Required] [MaxLength(500)] public string Description { get; set; }
}