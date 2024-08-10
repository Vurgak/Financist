using Financist.WebApi.Users.Application.Authentication.AuthenticateUser;
using Financist.WebApi.Users.Application.Authentication.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financist.WebApi.Users.Module.Controllers;

[ApiController]
[Route("authenticate")]
public class AuthenticationController(ISender sender) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> RegisterUserAsync(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Created();
    }

    [HttpPost]
    public async Task<ActionResult> AuthenticateUserAsync(AuthenticateUserCommand request, CancellationToken cancellationToken)
    {
        await sender.Send(request, cancellationToken);
        return Ok();
    }
}
