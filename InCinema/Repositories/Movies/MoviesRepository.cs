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
        IEnumerable<Movie> movies = connection.Query<Movie, Country, Movie>(sqlQuery, (movie, county) =>
        {
            movie.Country = county;
            return movie;
        });
        return movies;
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
        }, new {id}).FirstOrDefault();

        if (movie == null)
            throw new NotFoundException("Movie not found");

        return movie;
    }

    public void Add(Movie item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlString = @"insert into Movies values ( 
                           @Name, @Description,
                           @ReleaseDate, @Budget, 
                           @Duration, @CountryId) select @@IDENTITY";

        int movieId = connection.QuerySingle<int>(sqlString, new
        {
            item.Name, item.Description, item.ReleaseDate, item.Budget, item.Duration,
            CountryId = item.Country.Id
        });
        item.Id = movieId;
    }

    public void Update(Movie item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"update Movies set 
                  Name = @Name,  Description = @Description,
                  ReleaseDate = @ReleaseDate, Budget = @Budget,
                  Duration = @Duration,CountryId = @CountryId
                  where Id = @Id";

        connection.Execute(sqlQuery, new
        {
            item.Id, item.Name, item.Description, item.ReleaseDate, item.Budget, item.Duration,
            CountryId = item.Country.Id
        });
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        connection.Execute("delete from Movies where Id = @id", new {id});
    }
}