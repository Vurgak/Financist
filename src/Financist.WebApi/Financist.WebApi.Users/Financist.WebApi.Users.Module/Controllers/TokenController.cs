using Microsoft.AspNetCore.Mvc;

namespace Financist.WebApi.Users.Module.Controllers;

[ApiController]
[Route("token")]
public class TokenController : ControllerBase
{
    [HttpPost]
    public Task<ActionResult> RefreshTokenAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete]
    public Task<ActionResult> RevokeTokenAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
