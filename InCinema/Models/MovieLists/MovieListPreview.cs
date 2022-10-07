using InCinema.Models.Users;

namespace InCinema.Models.MovieLists;

public class MovieListPreview
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserPreview Author { get; set; }
}