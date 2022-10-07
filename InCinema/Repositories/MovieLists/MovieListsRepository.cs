using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.MovieLists;
using InCinema.Models.Users;

namespace InCinema.Repositories.MovieLists;

public class MovieListsRepository : IMovieListsRepository
{
    private readonly string _connectionKey;

    public MovieListsRepository(string connectionKey)
    {
        _connectionKey = connectionKey;
    }

    public IEnumerable<MovieList> GetAll()
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from MovieLists
                         inner join Users on MovieLists.UserId =Users.Id";

        return connection.Query<MovieList, User, MovieList>(sqlQuery,
            map: (movieList, user) =>
            {
                movieList.Author = user;
                return movieList;
            });
    }

    public MovieList GetById(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from MovieLists
                         inner join Users on MovieLists.UserId =Users.Id
                         where MovieLists.Id = @id";

        return connection.Query<MovieList, User, MovieList>(sqlQuery,
            map: (movieList, user) =>
            {
                movieList.Author = user;
                return movieList;
            }, new { id }).FirstOrDefault() ?? throw new NotFoundException("Movie list not found");
    }

    public void Add(MovieList item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "insert into MovieLists values (@Name, @IsPublic, @AuthorId) select @@identity";
        item.Id = connection.QuerySingle<int>(sqlQuery,
            new
            {
                item.Name, item.IsPublic, AuthorId = item.Author.Id
            });
    }

    public void Update(MovieList item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "update MovieLists set Name = @Name, IsPublic = @IsPublic where Id = @Id";
        connection.Execute(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "delete from MovieLists where Id = @id";
        connection.Execute(sqlQuery, new { id });
    }

    public IEnumerable<MovieList> GetPublicByUserId(int userId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from MovieLists
                         inner join Users on MovieLists.UserId =Users.Id
                         where MovieLists.IsPublic = 1 and MovieLists.UserId = @userId";

        return connection.Query<MovieList, User, MovieList>(sqlQuery,
            map: (movieList, user) =>
            {
                movieList.Author = user;
                return movieList;
            }, new { userId });
    }

    public IEnumerable<MovieList> GetAllByUserId(int userId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from MovieLists
                         inner join Users on MovieLists.UserId =Users.Id
                         where MovieLists.UserId = @userId";

        return connection.Query<MovieList, User, MovieList>(sqlQuery,
            map: (movieList, user) =>
            {
                movieList.Author = user;
                return movieList;
            }, new { userId });
    }

    public void AddMovie(int movieListId, int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "insert into MovieListsMovies values (@movieListId, @movieId)";
        connection.Execute(sqlQuery, new { movieListId, movieId });
    }

    public void DeleteMovie(int movieListId, int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "delete from MovieListsMovies where MovieListId = @movieListId and MovieId = @movieId";
        connection.Execute(sqlQuery, new { movieListId, movieId });
    }
}