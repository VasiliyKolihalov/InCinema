using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.Genres;

public class GenreCreate
{
    [Required] [MaxLength(50)] public string Name { get; set; }
    [Required] [MaxLength(500)] public string Description { get; set; }
}