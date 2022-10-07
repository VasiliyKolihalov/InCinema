using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Countries;
using InCinema.Models.Movies;

namespace InCinema.Repositories.Movies;

public class MoviesRepository : IMoviesRepository
{
    private readonly string _connectionKey;

    public MoviesRepository(string connectionKey)
    {
        _connectionKey = connectionKey;
    }

    public IEnumerable<Movie> GetAll()
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from Movies inner join Countries on Movies.CountryId = Countries.Id";
        return connection.Query<Movie, Country, Movie>(sqlQuery, (movie, county) =>
        {
            movie.Country = county;
            return movie;
        });
    }

    public Movie GetById(int id)
    {
        using var connection = new SqlConnection(_connectionKey);

        var sqlQuery = @"select * from Movies 
                           inner join Countries on Movies.CountryId = Countries.Id
                           where Movies.Id = @id";

        Movie? movie = connection.Query<Movie, Country, Movie>(sqlQuery, (movie, county) =>
        {
            movie.Country = county;
            return movie;
        }, new { id }).FirstOrDefault();

        return movie ?? throw new NotFoundException("Movie not found");
    }

    public void Add(Movie item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlString = @"insert into Movies values ( 
                          @Name, @Description,
                          @ReleaseDate, @Budget, 
                          @Duration, @CountryId, 
                          @DirectorId) select @@IDENTITY";

        item.Id = connection.QuerySingle<int>(sqlString, new
        {
            item.Name, item.Description, item.ReleaseDate,
            item.Budget, item.Duration, CountryId = item.Country.Id, item.DirectorId
        });
    }

    public void Update(Movie item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"update Movies set 
                         Name = @Name,  Description = @Description,
                         ReleaseDate = @ReleaseDate, Budget = @Budget,
                         Duration = @Duration, CountryId = @CountryId, 
                         DirectorId = @DirectorId
                         where Id = @Id";

        connection.Execute(sqlQuery, new
        {
            item.Id, item.Name, item.Description,
            item.ReleaseDate, item.Budget, item.Duration,
            CountryId = item.Country.Id, item.DirectorId
        });
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        connection.Execute("delete from Movies where Id = @id", new { id });
    }

    public IEnumerable<Movie> GetByDirectorId(int moviePersonId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from Movies
                         inner join Countries on Movies.CountryId = Countries.Id
                         where DirectorId = @moviePersonId";

        IEnumerable<Movie> movies = connection.Query<Movie, Country, Movie>(sqlQuery, (movie, country) =>
        {
            movie.Country = country;
            return movie;
        }, new { moviePersonId });

        return movies;
    }

    public IEnumerable<Movie> GetByActorId(int moviePersonId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from Movies
                         inner join Countries on Movies.CountryId = Countries.Id
                         inner join MoviesActors on Movies.Id = MoviesActors.MovieId
                         where MoviesActors.MoviePersonId = @moviePersonId";

        return connection.Query<Movie, Country, Movie>(sqlQuery, (movie, country) =>
        {
            movie.Country = country;
            return movie;
        }, new { moviePersonId });
    }

    public double? GetScore(int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var scoreQuery = @"select avg(cast(MovieScore as float)) from Reviews where MovieId = @movieId";
        return connection.QuerySingleOrDefault<double?>(scoreQuery, new { movieId });
    }

    public IEnumerable<Movie> GetByMovieListId(int movieListId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from Movies
                         inner join Countries on Movies.CountryId = Countries.Id
                         inner join MovieListsMovies on Movies.Id = MovieListsMovies.MovieId
                         where MovieListsMovies.MovieListId = @movieListId";

        return connection.Query<Movie, Country, Movie>(sqlQuery, (movie, country) =>
        {
            movie.Country = country;
            return movie;
        }, new { movieListId });
    }
}