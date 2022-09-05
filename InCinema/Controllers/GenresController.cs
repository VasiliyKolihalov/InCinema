using InCinema.Constants;
using InCinema.Models.Genres;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class GenresController : ControllerBase
{
    private readonly GenresService _genresService;

    public GenresController(GenresService genresService)
    {
        _genresService = genresService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<GenreView>> GetAll()
    {
        IEnumerable<GenreView> genreViews = _genresService.GetAll();
        return Ok(genreViews);
    }
    
    [HttpGet("{genreId}")]
    public ActionResult<GenreView> Get(int genreId)
    {
        GenreView genreView = _genresService.GetById(genreId);
        return Ok(genreView);
    }

    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpPost]
    public ActionResult<GenreView> Post(GenreCreate genreCreate)
    {
        GenreView genreView = _genresService.Create(genreCreate);
        return Ok(genreView);
    }

    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpPut]
    public ActionResult<GenreView> Put(GenreUpdate genreUpdate)
    {
        GenreView genreView = _genresService.Update(genreUpdate);
        return Ok(genreView);
    }

    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpDelete("{genreId}")]
    public ActionResult<GenreView> Delete(int genreId)
    {
        GenreView genreView = _genresService.Delete(genreId);
        return Ok(genreView);
    }
}