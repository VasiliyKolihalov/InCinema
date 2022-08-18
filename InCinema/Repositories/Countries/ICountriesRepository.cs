using InCinema.Models.Countries;

namespace InCinema.Repositories.Countries;

public interface ICountriesRepository
{
    public Country GetById(int countryId);
}