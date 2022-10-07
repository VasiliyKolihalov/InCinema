using InCinema.Models.Movies;
using InCinema.Models.Users;

namespace InCinema.Models.MovieLists;

public class MovieListView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserPreview Author { get; set; }
    public IEnumerable<MoviePreview> Movies { get; set; }
}