using InCinema.Repositories.Careers;
using InCinema.Repositories.Countries;
using InCinema.Repositories.Genres;
using InCinema.Repositories.MoviePersons;
using InCinema.Repositories.Movies;
using InCinema.Repositories.Users;

namespace InCinema.Repositories;

public class ApplicationContext : IApplicationContext
{
    public IMoviesRepository Movies { get; }
    public IGenresRepository Genres { get; }
    public ICountriesRepository Countries { get; }
    public IMoviePersonsRepository MoviePersons { get; }
    public ICareersRepository Careers { get; }
    public IUsersRepository Users { get; }

    public ApplicationContext(string connectionString)
    {
        Movies = new MoviesRepository(connectionString);
        Genres = new GenresRepository(connectionString);
        Countries = new CountriesRepository(connectionString);
        MoviePersons = new MoviePersonsRepository(connectionString);
        Careers = new CareersRepository(connectionString);
        Users = new UsersRepository(connectionString);
    }
}