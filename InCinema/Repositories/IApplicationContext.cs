using InCinema.Repositories.Careers;
using InCinema.Repositories.Countries;
using InCinema.Repositories.Genres;
using InCinema.Repositories.MovieLists;
using InCinema.Repositories.MoviePersons;
using InCinema.Repositories.Movies;
using InCinema.Repositories.Reviews;
using InCinema.Repositories.Roles;
using InCinema.Repositories.Users;

namespace InCinema.Repositories;

public interface IApplicationContext
{
    public IMoviesRepository Movies { get; }
    public IGenresRepository Genres { get; }
    public ICountriesRepository Countries { get; }
    public IMoviePersonsRepository MoviePersons { get; }
    public ICareersRepository Careers { get; }
    public IUsersRepository Users { get; }
    public IRolesRepository Roles { get; }
    public IReviewsRepository Reviews { get; }
    public IMovieListsRepository MovieLists { get; }
}