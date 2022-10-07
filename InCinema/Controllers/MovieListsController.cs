using InCinema.Constants;
using InCinema.Extensions;
using InCinema.Models.MovieLists;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MovieListsController : ControllerBase
{
    private readonly MovieListsService _movieListsService;

    public MovieListsController(MovieListsService movieListsService)
    {
        _movieListsService = movieListsService;
    }

    [Authorize(Roles = RoleNames.UsersAdministrator)]
    [HttpGet]
    public ActionResult<IEnumerable<MovieListPreview>> GetAll()
    {
        IEnumerable<MovieListPreview> movieListPreviews = _movieListsService.GetAll();
        return Ok(movieListPreviews);
    }

    [HttpGet("{movieListId}")]
    public ActionResult<MovieListView> Get(int movieListId)
    {
        MovieListView movieListView = _movieListsService.GetById(movieListId, this.GetUserId());
        return Ok(movieListView);
    }

    [HttpGet("getByUser/{authorId}")]
    public ActionResult<IEnumerable<MovieListPreview>> GetByUserId(int authorId)
    {
        IEnumerable<MovieListPreview> movieListPreviews = _movieListsService.GetByUserId(authorId, this.GetUserId());
        return Ok(movieListPreviews);
    }

    [HttpPost]
    public ActionResult<MovieListPreview> Post(MovieListCreate movieListCreate)
    {
        MovieListPreview movieListPreview = _movieListsService.Create(movieListCreate, this.GetUserId());
        return Ok(movieListPreview);
    }

    [HttpPut]
    public ActionResult<MovieListPreview> Put(MovieListUpdate movieListUpdate)
    {
        MovieListPreview movieListPreview = _movieListsService.Update(movieListUpdate, this.GetUserId());
        return Ok(movieListPreview);
    }

    [HttpDelete("{movieListId}")]
    public ActionResult<MovieListPreview> Delete(int movieListId)
    {
        MovieListPreview movieListPreview = _movieListsService.Delete(movieListId, this.GetUserId());
        return Ok(movieListPreview);
    }

    [Route("{movieListId}/movies/{movieId}")]
    [HttpPost]
    public ActionResult<MovieListPreview> AddMovie(int movieListId, int movieId)
    {
        MovieListPreview movieListPreview = _movieListsService.AddMovie(movieListId, movieId, this.GetUserId());
        return Ok(movieListPreview);
    }

    [Route("{movieListId}/movies/{movieId}")]
    [HttpDelete]
    public ActionResult<MovieListPreview> DeleteMovie(int movieListId, int movieId)
    {
        MovieListPreview movieListPreview = _movieListsService.DeleteMovie(movieListId, movieId, this.GetUserId());
        return Ok(movieListPreview);
    }
}