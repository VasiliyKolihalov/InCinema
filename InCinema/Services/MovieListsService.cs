using AutoMapper;
using InCinema.Constants;
using InCinema.Exceptions;
using InCinema.Models.MovieLists;
using InCinema.Models.Movies;
using InCinema.Models.Roles;
using InCinema.Models.Users;
using InCinema.Repositories;

namespace InCinema.Services;

public class MovieListsService
{
    private readonly IApplicationContext _applicationContext;
    private readonly IMapper _mapper;

    public MovieListsService(IApplicationContext applicationContext, IMapper mapper)
    {
        _applicationContext = applicationContext;
        _mapper = mapper;
    }

    public IEnumerable<MovieListPreview> GetAll()
    {
        IEnumerable<MovieList> movieLists = _applicationContext.MovieLists.GetAll();
        return _mapper.Map<IEnumerable<MovieListPreview>>(movieLists);
    }

    public IEnumerable<MovieListPreview> GetByUserId(int authorId, int userId)
    {
        _applicationContext.Users.GetById(authorId);
        IEnumerable<MovieList> movieLists = authorId == userId
            ? _applicationContext.MovieLists.GetAllByUserId(authorId)
            : _applicationContext.MovieLists.GetPublicByUserId(authorId);

        return _mapper.Map<IEnumerable<MovieListPreview>>(movieLists);
    }

    public MovieListView GetById(int movieListId, int userId)
    {
        MovieList movieList = _applicationContext.MovieLists.GetById(movieListId);
        if (!movieList.IsPublic && movieList.Author.Id != userId)
            throw new NotFoundException("Movie list not found");

        var movieListView = _mapper.Map<MovieListView>(movieList);
        IEnumerable<Movie> movies = _applicationContext.Movies.GetByMovieListId(movieListId);
        movieListView.Movies = _mapper.Map<IEnumerable<MoviePreview>>(movies);

        foreach (MoviePreview moviePreview in movieListView.Movies)
        {
            moviePreview.Score = _applicationContext.Movies.GetScore(moviePreview.Id);
        }
        return movieListView;
    }

    public MovieListPreview Create(MovieListCreate movieListCreate, int userId)
    {
        User user = _applicationContext.Users.GetById(userId);
        if (user.IsConfirmEmail == false)
            throw new BadRequestException("Only users with verified email can create movie lists");

        var movieList = _mapper.Map<MovieList>(movieListCreate);
        movieList.Author = user;

        _applicationContext.MovieLists.Add(movieList);

        return _mapper.Map<MovieListPreview>(movieList);
    }

    public MovieListPreview Update(MovieListUpdate movieListUpdate, int userId)
    {
        MovieList movieList = _applicationContext.MovieLists.GetById(movieListUpdate.Id);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);
        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && movieList.Author.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        MovieList updateMovieList = _mapper.Map<MovieListUpdate, MovieList>(movieListUpdate);
        updateMovieList.Author = movieList.Author;

        _applicationContext.MovieLists.Update(updateMovieList);

        return _mapper.Map<MovieListPreview>(updateMovieList);
    }

    public MovieListPreview Delete(int movieListId, int userId)
    {
        MovieList movieList = _applicationContext.MovieLists.GetById(movieListId);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);
        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && movieList.Author.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        _applicationContext.MovieLists.Delete(movieListId);

        return _mapper.Map<MovieListPreview>(movieList);
    }

    public MovieListPreview AddMovie(int movieListId, int movieId, int userId)
    {
        _applicationContext.Movies.GetById(movieId);
        MovieList movieList = _applicationContext.MovieLists.GetById(movieListId);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);
        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && movieList.Author.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        IEnumerable<Movie> movies = _applicationContext.Movies.GetByMovieListId(movieListId);
        if (movies.Any(x => x.Id == movieId))
            throw new BadRequestException("Movie list already contains this movie");

        _applicationContext.MovieLists.AddMovie(movieListId, movieId);

        return _mapper.Map<MovieListPreview>(movieList);
    }

    public MovieListPreview DeleteMovie(int movieListId, int movieId, int userId)
    {
        _applicationContext.Movies.GetById(movieId);
        MovieList movieList = _applicationContext.MovieLists.GetById(movieListId);

        IEnumerable<Role> roles = _applicationContext.Roles.GetByUserId(userId);
        if (roles.All(x => x.Name != RoleNames.UsersAdministrator) && movieList.Author.Id != userId)
            throw new ForbiddenException("User does not have enough rights for this action");

        IEnumerable<Movie> movies = _applicationContext.Movies.GetByMovieListId(movieListId);
        if (movies.All(x => x.Id != movieId))
            throw new BadRequestException("Movie list does not contain this movie");

        _applicationContext.MovieLists.DeleteMovie(movieListId, movieId);

        return _mapper.Map<MovieListPreview>(movieList);
    }
}