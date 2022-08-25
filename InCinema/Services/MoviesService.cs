using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Countries;
using InCinema.Models.Genres;
using InCinema.Models.MoviePersons;
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

    public MovieView GetById(int movieId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        var movieView = _mapper.Map<MovieView>(movie);
        
        MoviePerson director = _applicationContext.MoviePersons.GetById(movie.DirectorId);
        movieView.Director = _mapper.Map<MoviePersonPreview>(director);

        IEnumerable<MoviePerson> actors = _applicationContext.MoviePersons.GetActorsByMovie(movieId);
        movieView.Actors = _mapper.Map<IEnumerable<MoviePersonPreview>>(actors);

        IEnumerable<Genre> genres = _applicationContext.Genres.GetByMovie(movieId);
        movieView.Genres = _mapper.Map<IEnumerable<GenreView>>(genres);
        
        return movieView;
    }

    public MoviePreview Create(MovieCreate movieCreate)
    {
        _applicationContext.MoviePersons.GetById(movieCreate.DirectorId);
        
        Country country = _applicationContext.Countries.GetById(movieCreate.CountryId);
        var movie = _mapper.Map<Movie>(movieCreate);
        movie.Country = country;
        
        _applicationContext.Movies.Add(movie);
        
        return _mapper.Map<MoviePreview>(movie);
    }

    public MoviePreview Update(MovieUpdate movieUpdate)
    {
        _applicationContext.MoviePersons.GetById(movieUpdate.DirectorId);
        _applicationContext.Movies.GetById(movieUpdate.Id);
        
        Country country = _applicationContext.Countries.GetById(movieUpdate.CountryId);
        var movie = _mapper.Map<Movie>(movieUpdate);
        movie.Country = country;
        
        _applicationContext.Movies.Update(movie);

        return _mapper.Map<MoviePreview>(movie);
    }

    public MoviePreview Delete(int movieId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);

        _applicationContext.Movies.Delete(movieId);

        return _mapper.Map<MoviePreview>(movie);
    }

    #region Genres

    public MoviePreview AddGenre(int movieId, int genreId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        _applicationContext.Genres.GetById(genreId);

        IEnumerable<Genre> movieGenres = _applicationContext.Genres.GetByMovie(movieId);
        if (movieGenres.Any(genre => genre.Id == genreId))
            throw new BadRequestException("Movie already have this genre");

        _applicationContext.Genres.AddToMovie(genreId, movieId);

        return _mapper.Map<MoviePreview>(movie);
    }

    public MoviePreview DeleteGenre(int movieId, int genreId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);

        IEnumerable<Genre> movieGenres = _applicationContext.Genres.GetByMovie(movieId);
        if (movieGenres.All(genre => genre.Id != genreId))
            throw new BadRequestException("Movie does not have this genre");

        _applicationContext.Genres.DeleteFromMovies(genreId, movieId);

        return _mapper.Map<MoviePreview>(movie);
    }

    #endregion

    #region Actors

    public MoviePreview AddToActors(int movieId, int moviePersonId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        _applicationContext.MoviePersons.GetById(moviePersonId);

        IEnumerable<MoviePerson> actors = _applicationContext.MoviePersons.GetActorsByMovie(movieId);
        if (actors.Any(x => x.Id == moviePersonId))
            throw new BadRequestException("Movie-person already an actor in this movie");
        
        _applicationContext.MoviePersons.AddToMoviesActors(moviePersonId, movieId);

        return _mapper.Map<MoviePreview>(movie);
    }

    public MoviePreview DeleteFromActors(int movieId, int moviePersonId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        _applicationContext.MoviePersons.GetById(moviePersonId);

        IEnumerable<MoviePerson> actors = _applicationContext.MoviePersons.GetActorsByMovie(movieId);
        if (actors.All(x => x.Id != moviePersonId))
            throw new BadRequestException("Movie-person not an actor in this movie");
        
        _applicationContext.MoviePersons.DeleteFromMoviesActors(moviePersonId, movieId);

        return _mapper.Map<MoviePreview>(movie);
    }

    #endregion
}