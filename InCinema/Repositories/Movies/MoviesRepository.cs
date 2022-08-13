using System.Data;
using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
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
        using IDbConnection connection = new SqlConnection(_connectionKey);
        return connection.Query<Movie>("select * from Movies");
    }

    public Movie GetById(int id)
    {
        using IDbConnection connection = new SqlConnection(_connectionKey);

        var sqlQuery = "select * from Movies where Id = @id";
        var movie = connection.QuerySingleOrDefault<Movie>(sqlQuery, new {id}) ??
                    throw new NotFoundException("Movie not found");
        return movie;
    }

    public void Add(Movie item)
    {
        using IDbConnection connection = new SqlConnection(_connectionKey);
        var sqlString = "insert into Movies values (@Name, @ReleaseDate, @Budget, @Time) select @@IDENTITY";
        item.Id = connection.QuerySingle<int>(sqlString, item);
    }

    public void Update(Movie item)
    {
        using IDbConnection connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"update Movies set 
                  Name = @Name,  
                  ReleaseDate = @ReleaseDate, 
                  Budget = @Budget,
                  Time = @Time 
                  where Id = @Id";
        connection.Query(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using IDbConnection connection = new SqlConnection(_connectionKey);
        connection.Query("delete from Movies where Id = @id", new {id});
    }
}