using InCinema.Models.Movies;

namespace InCinema.Repositories.Movies;

public interface IMoviesRepository : IRepository<Movie, int>
{
    public IEnumerable<Movie> GetByDirectorId(int moviePersonId);
    public IEnumerable<Movie> GetByActorId(int moviePersonId);
}