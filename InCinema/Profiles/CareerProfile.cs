using AutoMapper;
using InCinema.Models.Careers;

namespace InCinema.Profiles;

public class CareerProfile : Profile
{
    public CareerProfile()
    {
        CreateMap<Career, CareerView>();
        CreateMap<CareerCreate, Career>();
        CreateMap<CareerUpdate, Career>();
    }
}