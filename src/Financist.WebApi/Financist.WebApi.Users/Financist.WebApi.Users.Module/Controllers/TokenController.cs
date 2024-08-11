using Financist.WebApi.Users.Application.Token.RefreshToken;
using Financist.WebApi.Users.Application.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Financist.WebApi.Users.Module.Controllers;

[ApiController]
[Route("token")]
public class TokenController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<TokensViewModel>> RefreshTokenAsync(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(request, cancellationToken);
        return result;
    }
    
    [HttpDelete]
    public Task<ActionResult> RevokeTokenAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
