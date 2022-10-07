using AutoMapper;
using InCinema.Constants;
using InCinema.Exceptions;
using InCinema.Models.Countries;
using InCinema.Models.Genres;
using InCinema.Models.MoviePersons;
using InCinema.Models.Movies;
using InCinema.Models.Reviews;
using InCinema.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace InCinema.Services;

public class MoviesService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _cache;

    public MoviesService(IApplicationContext applicationContext, IMapper mapper, IMemoryCache cache)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
        _cache = cache;
    }

    public IEnumerable<MoviePreview> GetAll()
    {
        IEnumerable<Movie> movies = _applicationContext.Movies.GetAll();
        var moviePreviews = _mapper.Map<IEnumerable<MoviePreview>>(movies);
        foreach (MoviePreview moviePreview in moviePreviews)
        {
            moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        }
        return moviePreviews;
    }

    public MovieView GetById(int movieId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        var movieView = _mapper.Map<MovieView>(movie);
        movieView.Score = GetMovieScoreFromCache(movieId);

        MoviePerson director = _applicationContext.MoviePersons.GetById(movie.DirectorId);
        movieView.Director = _mapper.Map<MoviePersonPreview>(director);

        IEnumerable<MoviePerson> actors = _applicationContext.MoviePersons.GetActorsByMovieId(movieId);
        movieView.Actors = _mapper.Map<IEnumerable<MoviePersonPreview>>(actors);

        IEnumerable<Genre> genres = _applicationContext.Genres.GetByMovie(movieId);
        movieView.Genres = _mapper.Map<IEnumerable<GenreView>>(genres);

        IEnumerable<Review> reviews = _applicationContext.Reviews.GetByMovieId(movieId);
        movieView.Reviews = _mapper.Map<IEnumerable<ReviewView>>(reviews);

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

        var moviePreview = _mapper.Map<MoviePreview>(movie);
        moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        return moviePreview;
    }

    public MoviePreview Delete(int movieId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);

        _applicationContext.Movies.Delete(movieId);

        var moviePreview = _mapper.Map<MoviePreview>(movie);
        moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        return moviePreview;
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

        var moviePreview = _mapper.Map<MoviePreview>(movie);
        moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        return moviePreview;
    }

    public MoviePreview DeleteGenre(int movieId, int genreId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);

        IEnumerable<Genre> movieGenres = _applicationContext.Genres.GetByMovie(movieId);
        if (movieGenres.All(genre => genre.Id != genreId))
            throw new BadRequestException("Movie does not have this genre");

        _applicationContext.Genres.DeleteFromMovies(genreId, movieId);

        var moviePreview = _mapper.Map<MoviePreview>(movie);
        moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        return moviePreview;
    }

    #endregion

    #region Actors

    public MoviePreview AddToActors(int movieId, int moviePersonId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        _applicationContext.MoviePersons.GetById(moviePersonId);

        IEnumerable<MoviePerson> actors = _applicationContext.MoviePersons.GetActorsByMovieId(movieId);
        if (actors.Any(x => x.Id == moviePersonId))
            throw new BadRequestException("Movie-person already an actor in this movie");

        _applicationContext.MoviePersons.AddToMoviesActors(moviePersonId, movieId);

        var moviePreview = _mapper.Map<MoviePreview>(movie);
        moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        return moviePreview;
    }

    public MoviePreview DeleteFromActors(int movieId, int moviePersonId)
    {
        Movie movie = _applicationContext.Movies.GetById(movieId);
        _applicationContext.MoviePersons.GetById(moviePersonId);

        IEnumerable<MoviePerson> actors = _applicationContext.MoviePersons.GetActorsByMovieId(movieId);
        if (actors.All(x => x.Id != moviePersonId))
            throw new BadRequestException("Movie-person not an actor in this movie");

        _applicationContext.MoviePersons.DeleteFromMoviesActors(moviePersonId, movieId);

        var moviePreview = _mapper.Map<MoviePreview>(movie);
        moviePreview.Score = GetMovieScoreFromCache(moviePreview.Id);
        return moviePreview;
    }

    #endregion

    private double? GetMovieScoreFromCache(int movieId)
    {
        if (_cache.TryGetValue(movieId, out double? score)) return score;
        score = _applicationContext.Movies.GetScore(movieId);
        _cache.Set(movieId, score, new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = CacheStorageTime.MovieScore
        });
        return score;
    }
}