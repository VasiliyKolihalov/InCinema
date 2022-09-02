using InCinema.Extensions;
using InCinema.Models.Users;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<UserPreview>> GetAll()
    {
        return Ok(_usersService.GetAll());
    }

    [HttpGet("{userId}")]
    public ActionResult<UserView> Get(int userId)
    {
        UserView userView = _usersService.GetById(userId);
        return Ok(userView);
    }

    [Authorize]
    [HttpPut]
    public ActionResult<UserPreview> Put(UserUpdate userUpdate)
    {
        UserPreview userView = _usersService.Update(userUpdate, this.GetUserId());
        return Ok(userView);
    }

    [HttpDelete("{userId}")]
    public ActionResult<UserPreview> Delete(int userId)
    {
        UserPreview userPreview = _usersService.Delete(userId);
        return Ok(userPreview);
    }
}