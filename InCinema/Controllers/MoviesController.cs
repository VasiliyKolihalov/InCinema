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
        IEnumerable<MoviePreview> movieViews = _moviesService.GetAll();
        return Ok(movieViews);
    }

    [HttpGet("{movieId}")]
    public ActionResult<MovieView> Get(int movieId)
    {
        MovieView movieView = _moviesService.Get(movieId);
        return Ok(movieView);
    }

    [HttpPost]
    public ActionResult<MovieView> Post(MovieCreate movieCreate)
    {
        MovieView movieView = _moviesService.Create(movieCreate);
        return Ok(movieView);
    }

    [HttpPut]
    public ActionResult<MovieView> Put(MovieUpdate movieUpdate)
    {
        MovieView movieView = _moviesService.Update(movieUpdate);
        return Ok(movieView);
    }

    [HttpDelete("{movieId}")]
    public ActionResult<MovieView> Delete(int movieId)
    {
        MovieView movieView = _moviesService.Delete(movieId);
        return Ok(movieView);
    }
}