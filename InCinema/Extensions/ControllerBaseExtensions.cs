using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace InCinema.Extensions;

public static class ControllerBaseExtensions
{
    public static int GetUserId(this ControllerBase controllerBase)
    {
        Claim userIdClaim = controllerBase.User.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier);
        int userId = Convert.ToInt32(userIdClaim.Value);
        return userId;
    }
}