using System.Data.SqlClient;
using Dapper;
using InCinema.Exceptions;
using InCinema.Models.Countries;

namespace InCinema.Repositories.Countries;

public class CountriesRepository : ICountriesRepository
{
    private readonly string _connectionKey;

    public CountriesRepository(string connectionKey)
    {
        _connectionKey = connectionKey;
    }

    public Country GetById(int countryId)
    {
        using var connection = new SqlConnection(_connectionKey);
        var sqlQuery = "select * from Countries where Id = @countryId";
        return connection.QueryFirstOrDefault<Country>(sqlQuery, new {countryId}) ??
               throw new NotFoundException("Country not found");
    }
}