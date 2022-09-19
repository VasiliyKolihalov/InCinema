using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Careers;

namespace InCinema.Repositories.Careers;

public class CareersRepository : ICareersRepository
{
    private readonly string _connectionString;

    public CareersRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IEnumerable<Career> GetAll()
    {
        using var connection = new SqlConnection(_connectionString);
        return connection.Query<Career>("select * from Careers");
    }

    public Career GetById(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "select * from Careers where Id = @id";
        return connection.QuerySingleOrDefault<Career>(sqlQuery, new {id}) ??
               throw new NotFoundException("Career not found");
    }

    public void Add(Career item)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "insert into Careers values (@Name, @Description) select @@IDENTITY";
        item.Id = connection.QuerySingle<int>(sqlQuery, item);
    }

    public void Update(Career item)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "update Careers set Name = @Name, Description = @Description where Id = @Id";
        connection.Execute(sqlQuery, item);
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("delete from Careers where Id = @id", new {id});
    }

    public Career? GetByName(string careerName)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "select * from Careers where Name = @careerName";
        return connection.QueryFirstOrDefault<Career>(sqlQuery, new {careerName});
    }

    public IEnumerable<Career> GetByMoviePerson(int moviePersonId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = @"select * from Careers
                         inner join MoviePersonsCareers as mpc on mpc.CareerId = Careers.Id
                         where mpc.MoviePersonId = @moviePersonId";

        return connection.Query<Career>(sqlQuery, new {moviePersonId});
    }

    public void AddToMoviePerson(int careerId, int moviePersonId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = "insert into MoviePersonsCareers values (@moviePersonId, @careerId)";
        connection.Execute(sqlQuery, new {moviePersonId, careerId});
    }

    public void DeleteFromMoviePerson(int careerId, int moviePersonId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sqlQuery = @"delete from MoviePersonsCareers 
                         where MoviePersonId = @moviePersonId 
                         and CareerId = @careerId";

        connection.Execute(sqlQuery, new {moviePersonId, careerId});
    }
}