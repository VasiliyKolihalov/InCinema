using InCinema.Models.MovieLists;

namespace InCinema.Repositories.MovieLists;

public interface IMovieListsRepository : IRepository<MovieList, int>
{
    public IEnumerable<MovieList> GetPublicByUserId(int userId);
    public IEnumerable<MovieList> GetAllByUserId(int userId);

    public void AddMovie(int movieListId, int movieId);
    public void DeleteMovie(int movieListId, int movieId);
}