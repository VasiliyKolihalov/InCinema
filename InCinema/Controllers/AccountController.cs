using InCinema.Models.Users;
using InCinema.Services;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;

    public AccountController(AccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public ActionResult Post(UserCreate userCreate)
    {
        string jwt = _accountService.Register(userCreate);
        return Ok(new { token = jwt });
    }

    [Route("login")]
    [HttpPost]
    public ActionResult Login(UserLogin userLogin)
    {
        string jwt = _accountService.Login(userLogin);
        return Ok(new { token = jwt });
    }
    
    [Route("changePassword")]
    [HttpPut]
    public ActionResult ChangePassword(ChangeUserPassword changeUserPassword)
    {
        string jwt = _accountService.ChangePassword(changeUserPassword);
        return Ok(new { token = jwt });
    }
}