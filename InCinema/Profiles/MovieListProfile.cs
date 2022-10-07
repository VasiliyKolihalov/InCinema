using AutoMapper;
using InCinema.Models.MovieLists;

namespace InCinema.Profiles;

public class MovieListProfile : Profile
{
    public MovieListProfile()
    {
        CreateMap<MovieList, MovieListPreview>();
        CreateMap<MovieList, MovieListView>();
        CreateMap<MovieListCreate, MovieList>();
        CreateMap<MovieListUpdate, MovieList>();
    }
}