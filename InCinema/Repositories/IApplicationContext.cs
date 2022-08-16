using InCinema.Repositories.Genres;
using InCinema.Repositories.Movies;

namespace InCinema.Repositories;

public interface IApplicationContext
{
    public IMoviesRepository Movies { get; }
    public IGenresRepository Genres { get; }
}