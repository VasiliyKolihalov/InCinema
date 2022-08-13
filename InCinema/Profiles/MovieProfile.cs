using AutoMapper;
using InCinema.Models.Movies;

namespace InCinema.Profiles;

public class MovieProfile : Profile
{
    public MovieProfile()
    {
        CreateMap<Movie, MoviePreview>();
        CreateMap<Movie, MovieView>();
        CreateMap<MovieCreate, Movie>();
        CreateMap<MovieUpdate, Movie>();
    }
}