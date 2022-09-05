using InCinema.Constants;
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

    [Authorize]
    [HttpDelete("{userToDeleteId}")]
    public ActionResult<UserPreview> Delete(int userToDeleteId)
    {
        UserPreview userPreview = _usersService.Delete(userToDeleteId, this.GetUserId());
        return Ok(userPreview);
    }

    [Authorize(Roles = RoleNames.UsersAdministrator)]
    [Route("{userId}/roles/{roleId}")]
    [HttpPost]
    public ActionResult<UserPreview> AddRole(int userId, int roleId)
    {
        UserPreview userPreview = _usersService.AddRole(userId, roleId);
        return Ok(userPreview);
    }
    
    [Authorize(Roles = RoleNames.UsersAdministrator)]
    [Route("{userId}/roles/{roleId}")]
    [HttpDelete]
    public ActionResult<UserPreview> DeleteRole(int userId, int roleId)
    {
        UserPreview userPreview = _usersService.DeleteRole(userId, roleId);
        return Ok(userPreview);
    }
}