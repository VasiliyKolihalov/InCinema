using AutoMapper;
using InCinema.Models.MoviePersons;

namespace InCinema.Profiles;

public class MoviePersonProfile : Profile
{
    public MoviePersonProfile()
    {
        CreateMap<MoviePerson, MoviePersonPreview>();
        CreateMap<MoviePerson, MoviePersonView>();
        CreateMap<MoviePersonCreate, MoviePerson>();
        CreateMap<MoviePersonUpdate, MoviePerson>();
    }
}