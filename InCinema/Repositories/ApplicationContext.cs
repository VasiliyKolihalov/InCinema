using InCinema.Repositories.Movies;

namespace InCinema.Repositories;

public class ApplicationContext : IApplicationContext
{
    public IMoviesRepository Movies { get; }

    public ApplicationContext(string connectionString)
    {
        Movies = new MoviesRepository(connectionString);
    }
}