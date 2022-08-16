using InCinema.Repositories.Genres;
using InCinema.Repositories.Movies;

namespace InCinema.Repositories;

public class ApplicationContext : IApplicationContext
{
    public IMoviesRepository Movies { get; }
    public IGenresRepository Genres { get; }

    public ApplicationContext(string connectionString)
    {
        Movies = new MoviesRepository(connectionString);
        Genres = new GenresRepository(connectionString);
    }
}