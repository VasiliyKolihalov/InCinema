using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Reviews;
using InCinema.Models.Users;

namespace InCinema.Repositories.Reviews;

public class ReviewsRepository : IReviewsRepository
{
    private readonly string _connectionKey;

    public ReviewsRepository(string connectionKey)
    {
        _connectionKey = connectionKey;
    }

    public IEnumerable<Review> GetAll()
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from Reviews inner join Users on Reviews.UserId = Users.Id";
        return connection.Query<Review, User, Review>(sqlQuery, (review, user) =>
        {
            review.Author = user;
            return review;
        });
    }

    public Review GetById(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from Reviews inner join Users on Reviews.UserId = Users.Id where Reviews.Id = @id";
        Review? review = connection.Query<Review, User, Review>(sqlQuery, (review, user) =>
        {
            review.Author = user;
            return review;
        }, new { id }).FirstOrDefault();

        return review ?? throw new NotFoundException("Review not found");
    }

    public void Add(Review item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"insert into Reviews values (@MovieId, @MovieScore, @Text, @DateTime, @AuthorId) 
                         select @@identity";
        item.Id = connection.QuerySingle<int>(sqlQuery, new
        {
            item.MovieId, item.MovieScore,
            item.Text, item.DateTime,
            AuthorId = item.Author.Id
        });
    }

    public void Update(Review item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "update Reviews set MovieScore = @MovieScore, Text = @Text where Id = @Id";
        connection.Execute(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "delete from Reviews where Id = @id";
        connection.Execute(sqlQuery, new { id });
    }

    public IEnumerable<Review> GetByMovieId(int movieId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from Reviews 
                         inner join Users on Reviews.UserId = Users.Id 
                         where Reviews.MovieId = @movieId";
        
        return connection.Query<Review, User, Review>(sqlQuery, (review, user) =>
        {
            review.Author = user;
            return review;
        }, new { movieId });
    }

    public IEnumerable<Review> GetByUserId(int userId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from Reviews 
                         inner join Users on Reviews.UserId = Users.Id 
                         where Reviews.UserId = @userId";

        return connection.Query<Review, User, Review>(sqlQuery, (review, user) =>
        {
            review.Author = user;
            return review;
        }, new { userId });
    }

    public Review? GetUserReview(int movieId, int userId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from Reviews where MovieId = @movieId and UserId = @userId";
        return connection.QuerySingleOrDefault<Review>(sqlQuery, new { movieId, userId });
    }
}