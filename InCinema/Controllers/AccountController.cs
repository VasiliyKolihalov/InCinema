using InCinema.Extensions;
using InCinema.Models.Users;
using InCinema.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Route("password/change")]
    [HttpPut]
    public ActionResult ChangePassword(ChangeUserPassword changeUserPassword)
    {
        string jwt = _accountService.ChangePassword(changeUserPassword);
        return Ok(new { token = jwt });
    }

    [Route("email/change")]
    [HttpPut]
    public ActionResult ChangeEmail(ChangeUserEmail changeUserEmail)
    {
        string jwt = _accountService.ChangeEmail(changeUserEmail);
        return Ok(new { token = jwt });
    }

    [Authorize]
    [Route("email/sendConfirmCode")]
    public ActionResult SendEmailConfirmCode()
    {
        int userId = this.GetUserId();
        string code = _accountService.GenerateEmailConfirmCode(userId);
        string callbackUrl = Url.Action(action: nameof(ConfirmEmail),
            controller: "Account",
            values: new { userId, code },
            protocol: HttpContext.Request.Scheme)!;

        _accountService.SendEmailConfirmCode(userId, callbackUrl);
        return Ok();
    }

    [AllowAnonymous]
    [Route("email/confirm")]
    [HttpGet]
    public ActionResult ConfirmEmail(int userId, string code)
    {
        _accountService.ConfirmEmail(userId, code);
        return Ok();
    }
}