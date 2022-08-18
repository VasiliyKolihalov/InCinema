using AutoMapper;
using InCinema.Models.Countries;

namespace InCinema.Profiles;

public class CountyProfile : Profile
{
    public CountyProfile()
    {
        CreateMap<Country, CountryView>();
    }
}