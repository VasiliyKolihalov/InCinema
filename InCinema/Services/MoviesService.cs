using AutoMapper;
using InCinema.Models.Movies;
using InCinema.Repositories;

namespace InCinema.Services;

public class MoviesService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public MoviesService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<MoviePreview> GetAll()
    {
        IEnumerable<Movie> movies = _applicationContext.Movies.GetAll();
        return _mapper.Map<IEnumerable<MoviePreview>>(movies);
    }

    public MovieView Get(int id)
    {
        Movie movie = _applicationContext.Movies.GetById(id);
        return _mapper.Map<MovieView>(movie);
    }

    public MovieView Create(MovieCreate movieCreate)
    {
        Movie movie = _mapper.Map<Movie>(movieCreate);
        _applicationContext.Movies.Add(movie);

        return _mapper.Map<MovieView>(movie);
    }

    public MovieView Update(MovieUpdate movieUpdate)
    {
        _applicationContext.Movies.GetById(movieUpdate.Id);
        Movie movie = _mapper.Map<Movie>(movieUpdate);
        _applicationContext.Movies.Update(movie);

        return _mapper.Map<MovieView>(movie);
    }

    public MovieView Delete(int id)
    {
        Movie movie = _applicationContext.Movies.GetById(id);
        
        _applicationContext.Movies.Delete(id);

        return _mapper.Map<MovieView>(movie);
    }
}