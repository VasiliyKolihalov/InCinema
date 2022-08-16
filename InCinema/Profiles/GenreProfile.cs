using AutoMapper;
using InCinema.Models.Genres;

namespace InCinema.Profiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreView>();
        CreateMap<GenreCreate, Genre>();
        CreateMap<GenreUpdate, Genre>();
    }
}