using InCinema.Models.MoviePersons;
using InCinema.Models.Movies;
using InCinema.Services;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviePersonsController : ControllerBase
{
    private readonly MoviePersonsService _moviePersonsService;

    public MoviePersonsController(MoviePersonsService moviePersonsService)
    {
        _moviePersonsService = moviePersonsService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<MoviePersonPreview>> GetAll()
    {
        IEnumerable<MoviePersonPreview> moviePersonPreviews = _moviePersonsService.GetAll();
        return Ok(moviePersonPreviews);
    }

    [HttpGet("{moviePersonId}")]
    public ActionResult<MoviePersonView> Get(int moviePersonId)
    {
        MoviePersonView moviePersonView = _moviePersonsService.GetById(moviePersonId);
        return Ok(moviePersonView);
    }

    [HttpPost]
    public ActionResult<MoviePersonPreview> Post(MoviePersonCreate moviePersonCreate)
    {
        MoviePersonPreview moviePersonPreview = _moviePersonsService.Create(moviePersonCreate);
        return Ok(moviePersonPreview);
    }

    [HttpPut]
    public ActionResult<MoviePersonPreview> Put(MoviePersonUpdate moviePersonUpdate)
    {
        MoviePersonPreview moviePersonPreview = _moviePersonsService.Update(moviePersonUpdate);
        return Ok(moviePersonPreview);
    }

    [HttpDelete("{moviePersonId}")]
    public ActionResult<MoviePersonPreview> Delete(int moviePersonId)
    {
        MoviePersonPreview moviePersonPreview = _moviePersonsService.Delete(moviePersonId);
        return Ok(moviePersonPreview);
    }
    
    #region Careers

    [Route("{moviePersonId}/careers/{careerId}")]
    [HttpPost]
    public ActionResult<MoviePersonPreview> AddCareer(int moviePersonId, int careerId)
    {
        MoviePersonPreview moviePersonPreview = _moviePersonsService.AddCareer(moviePersonId, careerId);
        return Ok(moviePersonPreview);
    }
    
    [Route("{moviePersonId}/careers/{careerId}")]
    [HttpDelete]
    public ActionResult<MoviePersonPreview> DeleteCareer(int moviePersonId, int careerId)
    {
        MoviePersonPreview moviePersonPreview = _moviePersonsService.DeleteCareer(moviePersonId, careerId);
        return Ok(moviePersonPreview);
    }

    #endregion

    #region Work

    [Route("{moviePersonId}/WorkAsDirector")]
    [HttpGet]
    public ActionResult<IEnumerable<MoviePreview>> GetWorkAsDirector(int moviePersonId)
    {
        IEnumerable<MoviePreview> moviePreviews = _moviePersonsService.GetWorkAsDirector(moviePersonId);
        return Ok(moviePreviews);
    }
    
    [Route("{moviePersonId}/WorkAsActor")]
    [HttpGet]
    public ActionResult<IEnumerable<MoviePreview>> GetWorkAsActor(int moviePersonId)
    {
        IEnumerable<MoviePreview> moviePreviews = _moviePersonsService.GetWorkAsActor(moviePersonId);
        return Ok(moviePreviews);
    }

    #endregion
}