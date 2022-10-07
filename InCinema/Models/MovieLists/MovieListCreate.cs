using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.MovieLists;

public class MovieListCreate
{
    [Required] [MaxLength(100)] public string Name { get; set; }
    [Required] public bool IsPublic { get; set; }
}