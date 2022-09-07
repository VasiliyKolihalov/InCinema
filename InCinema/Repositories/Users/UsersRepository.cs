using System.Data;
using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Countries;
using InCinema.Models.Users;

namespace InCinema.Repositories.Users;

public class UsersRepository : IUsersRepository
{
    private readonly string _connectionString;

    public UsersRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<User> GetAll()
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<User>("select * from Users");
    }

    public User GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "select * from Users where Id = @id";
        return connection.QuerySingleOrDefault<User>(sqlQuery, new {id}) ??
               throw new NotFoundException("User not found");
    }

    public void Add(User item)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "insert into Users values(@FirstName, @LastName, @Email, @PasswordHash) select @@IDENTITY";
        int userId = connection.QuerySingle<int>(sqlQuery, item);
        item.Id = userId;
    }

    public void Update(User item)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "update Users set FirstName = @FirstName, LastName = @LastName where Id = @Id";
        connection.Execute(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("delete from Users where Id = @id", new {id});
    }

    public User? GetByEmail(string email)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "select * from Users where Email = @email";
        return connection.QuerySingleOrDefault<User>(sqlQuery, new {email});
    }

    public void ChangePasswordHash(User user)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "update Users set PasswordHash = @PasswordHash where Id = @Id";
        connection.Execute(sqlQuery, user);
    }
}