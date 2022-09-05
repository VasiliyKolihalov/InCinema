using InCinema.Constants;
using InCinema.Models.Careers;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class CareersController : ControllerBase
{
    private readonly CareersService _careersService;

    public CareersController(CareersService careersService)
    {
        _careersService = careersService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CareerView>> GetAll()
    {
        IEnumerable<CareerView> careerViews = _careersService.GetAll();
        return Ok(careerViews);
    }

    [HttpGet("{careerId}")]
    public ActionResult<CareerView> Get(int careerId)
    {
        CareerView careerView = _careersService.GetById(careerId);
        return Ok(careerView);
    }
    
    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpPost]
    public ActionResult<CareerView> Post(CareerCreate careerCreate)
    {
        CareerView careerView = _careersService.Create(careerCreate);
        return Ok(careerView);
    }
    
    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpPut]
    public ActionResult<CareerView> Put(CareerUpdate careerUpdate)
    {
        CareerView careerView = _careersService.Update(careerUpdate);
        return Ok(careerView);
    }
    
    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpDelete("{careerId}")]
    public ActionResult<CareerView> Delete(int careerId)
    {
        CareerView careerView = _careersService.Delete(careerId);
        return Ok(careerView);
    }
}