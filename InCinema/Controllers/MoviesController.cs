using InCinema.Models.Movies;
using InCinema.Services;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly MoviesService _moviesService;

    public MoviesController(MoviesService moviesService)
    {
        _moviesService = moviesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MoviePreview>> GetAll()
    {
        IEnumerable<MoviePreview> moviePreviews = _moviesService.GetAll();
        return Ok(moviePreviews);
    }

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

    [Route("{movieId}/genres/{genreId}/add")]
    [HttpPost]
    public ActionResult<MoviePreview> AddGenre(int movieId, int genreId)
    {
        MoviePreview moviePreview = _moviesService.AddGenre(movieId, genreId);
        return Ok(moviePreview);
    }
    
    [Route("{movieId}/genres/{genreId}/delete")]
    [HttpDelete]
    public ActionResult<MoviePreview> DeleteGenre(int movieId, int genreId)
    {
        MoviePreview moviePreview = _moviesService.DeleteGenre(movieId, genreId);
        return Ok(moviePreview);
    }

    #endregion
}