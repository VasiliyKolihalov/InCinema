using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Roles;

namespace InCinema.Repositories.Roles;

public class RolesRepository : IRolesRepository
{
    private readonly string _connectionString;

    public RolesRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Role> GetAll()
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Role>("select * from Roles");
    }

    public Role GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "select * from Roles where Id = @id";
        return connection.QuerySingleOrDefault<Role>(sqlQuery, new {id})
               ?? throw new NotFoundException("Role not found");
    }

    public void Add(Role item)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "insert into Roles values (@Name) select @@IDENTITY";
        int roleId = connection.QuerySingle<int>(sqlQuery, item);
        item.Id = roleId;
    }

    public void Update(Role item)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "update Roles set Name = @Name where Id = @Id";
        connection.Execute(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("delete from Roles where Id = @id", new {id});
    }

    public Role? GetByName(string roleName)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "select * from Roles where Name = @roleName";
        return connection.QuerySingleOrDefault<Role>(sqlQuery, new {roleName});
    }

    public IEnumerable<Role> GetByUserId(int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = @"select * from Roles
                         inner join UsersRoles on Roles.Id = UsersRoles.RoleId
                         where UsersRoles.UserId = @userId";
        return connection.Query<Role>(sqlQuery, new {userId});
    }

    public void AddToUser(int roleId, int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "insert into UsersRoles values (@userId, @roleId)";
        connection.Execute(sqlQuery, new {userId, roleId});
    }

    public void DeleteFromUser(int roleId, int userId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "delete from UsersRoles where UserId = @userId and RoleId = @RoleId";
        connection.Execute(sqlQuery, new {userId, roleId});
    }
}