using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financist.WebApi.Bootstrapper.Controllers;

[ApiController]
[Route("tests")]
public class TestsController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public ActionResult GetAsync()
    {
        return Ok("Hello, world!");
    }
}
