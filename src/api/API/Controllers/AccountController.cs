using Application.Common.Models;
using Application.Requests.Authentication.Commands;
using Application.Requests.Authentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{
    [HttpPost("GetAccessToken")]
    public async Task<ActionResult<Response<AuthenticateResponse>>> GetAccessToken(LoginUserRequest model)
    {
        var result = await Mediator.Send(new LoginUserCommand(model));
        return Ok(result);
    }

    [HttpPost("GetRefreshToken")]
    public async Task<ActionResult<AuthenticateResponse>> GetRefreshToken(RefreshRequest model)
    {
        var result = await Mediator.Send(new RefreshCommand(model));
        return Ok(result);
    }
}