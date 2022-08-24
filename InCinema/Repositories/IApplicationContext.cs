using InCinema.Repositories.Careers;
using InCinema.Repositories.Countries;
using InCinema.Repositories.Genres;
using InCinema.Repositories.MoviePersons;
using InCinema.Repositories.Movies;

namespace InCinema.Repositories;

public interface IApplicationContext
{
    public IMoviesRepository Movies { get; }
    public IGenresRepository Genres { get; }
    public ICountriesRepository Countries { get; }
    public IMoviePersonsRepository MoviePersons { get; }
    public ICareersRepository Careers { get; }
}