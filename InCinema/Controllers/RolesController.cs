using InCinema.Constants;
using InCinema.Models.Roles;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class RolesController : ControllerBase
{
    private readonly RolesService _rolesService;

    public RolesController(RolesService rolesService)
    {
        _rolesService = rolesService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RoleView>> GetAll()
    {
        return Ok(_rolesService.GetAll());
    }

    [HttpGet("{roleId}")]
    public ActionResult<RoleView> Get(int roleId)
    {
        RoleView roleView = _rolesService.GetById(roleId);
        return Ok(roleView);
    }

    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpPost]
    public ActionResult<RoleView> Post(RoleCreate roleCreate)
    {
        RoleView roleView = _rolesService.Create(roleCreate);
        return Ok(roleView);
    }
    
    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpPut]
    public ActionResult<RoleView> Put(RoleUpdate roleUpdate)
    {
        RoleView roleView = _rolesService.Update(roleUpdate);
        return Ok(roleView);
    }
    
    [Authorize(Roles = RoleNames.ContentAdministrator)]
    [HttpDelete("{roleId}")]
    public ActionResult<RoleView> Delete(int roleId)
    {
        RoleView roleView = _rolesService.Delete(roleId);
        return Ok(roleView);
    }
}