using InCinema.Constants;
using InCinema.Models.Movies;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = RoleNames.ContentAdministrator)]
public class MoviesController : ControllerBase
{
    private readonly MoviesService _moviesService;

    public MoviesController(MoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult<IEnumerable<MoviePreview>> GetAll()
    {
        IEnumerable<MoviePreview> moviePreviews = _moviesService.GetAll();
        return Ok(moviePreviews);
    }

    [AllowAnonymous]
    [HttpGet("{movieId}")]
    public ActionResult<MovieView> Get(int movieId)
    {
        MovieView movieView = _moviesService.GetById(movieId);
        return Ok(movieView);
    }

    [HttpPost]
    public ActionResult<MoviePreview> Post(MovieCreate movieCreate)
    {
        MoviePreview moviePreview = _moviesService.Create(movieCreate);
        return Ok(moviePreview);
    }

    [HttpPut]
    public ActionResult<MoviePreview> Put(MovieUpdate movieUpdate)
    {
        MoviePreview moviePreview = _moviesService.Update(movieUpdate);
        return Ok(moviePreview);
    }

    [HttpDelete("{movieId}")]
    public ActionResult<MoviePreview> Delete(int movieId)
    {
        MoviePreview moviePreview = _moviesService.Delete(movieId);
        return Ok(moviePreview);
    }

    #region Genres

    [Route("{movieId}/genres/{genreId}")]
    [HttpPost]
    public ActionResult<MoviePreview> AddGenre(int movieId, int genreId)
    {
        MoviePreview moviePreview = _moviesService.AddGenre(movieId, genreId);
        return Ok(moviePreview);
    }
    
    [Route("{movieId}/genres/{genreId}")]
    [HttpDelete]
    public ActionResult<MoviePreview> DeleteGenre(int movieId, int genreId)
    {
        MoviePreview moviePreview = _moviesService.DeleteGenre(movieId, genreId);
        return Ok(moviePreview);
    }

    #endregion
    
    #region Actors

    [Route("{movieId}/actors/{moviePersonId}")]
    [HttpPost]
    public ActionResult<MoviePreview> AddToActors(int movieId, int moviePersonId)
    {
        MoviePreview moviePreview = _moviesService.AddToActors(movieId, moviePersonId);
        return Ok(moviePreview);
    }
    
    [Route("{movieId}/actors/{moviePersonId}")]
    [HttpDelete]
    public ActionResult<MoviePreview> DeleteFromActors(int movieId, int moviePersonId)
    {
        MoviePreview moviePreview = _moviesService.DeleteFromActors(movieId, moviePersonId);
        return Ok(moviePreview);
    }

    #endregion
}