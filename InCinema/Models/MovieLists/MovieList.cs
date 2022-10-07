using InCinema.Models.Users;

namespace InCinema.Models.MovieLists;

public class MovieList
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public User Author { get; set; }
}