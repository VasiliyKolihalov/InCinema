using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Genres;

namespace InCinema.Repositories.Genres;

public class GenresRepository : IGenresRepository
{
    private readonly string _connectionKey;

    public GenresRepository(string connectionKey)
    {
        _connectionKey = connectionKey;
    }

    public IEnumerable<Genre> GetAll()
    {
        using var connection = new SqlConnection(_connectionKey);
        return connection.Query<Genre>("select * from Genres");
    }

    public Genre GetById(int id)
    {
        using var connection = new SqlConnection(_connectionKey);

        var sqlQuery = "select * from Genres where Id = @id";
        var genre = connection.QuerySingleOrDefault<Genre>(sqlQuery, new {id}) ??
                    throw new NotFoundException("Genre not found");
        return genre;
    }

    public void Add(Genre item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "insert into Genres values (@Name, @Description) select @@IDENTITY";
        item.Id = connection.QuerySingle<int>(sqlQuery, item);
    }

    public void Update(Genre item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "update Genres set Name = @Name, Description = @Description where Id = @Id";
        connection.Query(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        connection.Query("delete from Genres where Id = @id", new {id});
    }

    public Genre? GetByName(string genreName)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from genres where Name = @genreName";
        return connection.QueryFirstOrDefault<Genre>(sqlQuery, new {genreName});
    }

    public IEnumerable<Genre> GetByMovie(int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from Genres
                        inner join MoviesGenres on Genres.Id = MoviesGenres.GenreId 
                        and MoviesGenres.MovieId = @movieId";

        return connection.Query<Genre>(sqlQuery, new {movieId});
    }

    public void AddToMovie(int genreId, int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        connection.Query("insert into MoviesGenres values(@movieId, @genreId)", new {genreId, movieId});
    }

    public void DeleteFromMovies(int genreId, int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "delete from MoviesGenres where MovieId = @movieId and GenreId = @genreId";
        connection.Query(sqlQuery, new {movieId, genreId});
    }
}