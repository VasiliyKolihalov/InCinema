using System.ComponentModel.DataAnnotations;

namespace InCinema.Models.MovieLists;

public class MovieListUpdate
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; }
    [Required] public bool IsPublic { get; set; }
}