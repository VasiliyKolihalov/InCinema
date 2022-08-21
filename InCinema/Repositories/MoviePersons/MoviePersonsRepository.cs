using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Countries;
using InCinema.Models.MoviePersons;

namespace InCinema.Repositories.MoviePersons;

public class MoviePersonsRepository : IMoviePersonsRepository
{
    private readonly string _connectionKey;

    public MoviePersonsRepository(string connectionKey)
    {
        _connectionKey = connectionKey;
    }

    public IEnumerable<MoviePerson> GetAll()
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from MoviePersons inner join Countries on MoviePersons.CountryId = Countries.Id";
        IEnumerable<MoviePerson> moviePersons = connection.Query<MoviePerson, Country, MoviePerson>(sqlQuery,
            map: (person, country) =>
            {
                person.Country = country;
                return person;
            });
        return moviePersons;
    }

    public MoviePerson GetById(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"select * from MoviePersons 
                         inner join Countries on MoviePersons.CountryId = Countries.Id 
                         where MoviePersons.Id = @id";
        MoviePerson? moviePerson = connection.Query<MoviePerson, Country, MoviePerson>(sqlQuery, (person, country) =>
        {
            person.Country = country;
            return person;
        }, new {id}).FirstOrDefault();

        if (moviePerson == null)
            throw new NotFoundException("Movie not found");

        return moviePerson;
    }

    public void Add(MoviePerson item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"insert into MoviePersons values (
                         @FirstName, @LastName, 
                         @BirthDate, @CountryId) select @@IDENTITY";

        int moviePersonId = connection.QuerySingle<int>(sqlQuery, new
        {
            item.FirstName, item.LastName,
            item.BirthDate, CountryId = item.Country.Id
        });
        item.Id = moviePersonId;
    }

    public void Update(MoviePerson item)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = @"update MoviePersons set 
                         FirstName = @FirstName, LastName = @LastName,
                         BirthDate = @BirthDate, CountryId= @CountryId
                         where Id = @Id";
        
        connection.Execute(sqlQuery, new
        {
            item.Id, item.FirstName,
            item.LastName, item.BirthDate,
            CountryId = item.Country.Id
        });
    }

    public void Delete(int id)
    {
        using var connection = new SqlConnection(_connectionKey);
        connection.Execute("delete from MoviePersons where Id = @id", new {id});
    }
}