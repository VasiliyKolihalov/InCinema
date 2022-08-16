﻿using AutoMapper;
using InCinema.Exceptions;
using InCinema.Models.Genres;
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
        IEnumerable<Genre> genres = _applicationContext.Genres.GetByMovie(movieId);
        movieView.Genres = _mapper.Map<List<GenreView>>(genres);

        return movieView;
    }

    public MoviePreview Create(MovieCreate movieCreate)
    {
        var movie = _mapper.Map<Movie>(movieCreate);
        _applicationContext.Movies.Add(movie);

        return _mapper.Map<MoviePreview>(movie);
    }

    public MoviePreview Update(MovieUpdate movieUpdate)
    {
        _applicationContext.Movies.GetById(movieUpdate.Id);
        var movie = _mapper.Map<Movie>(movieUpdate);
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
}