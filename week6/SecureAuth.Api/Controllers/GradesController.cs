using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SecureAuth.Api.Controllers;

// Task 6.15's role-gated endpoint. [Authorize] alone would only require
// "logged in as anyone"; [Authorize(Roles = "Teacher")] additionally
// requires the token's role claim to be exactly "Teacher" — enforced by
// the ASP.NET Core authorization middleware server-side, not by the
// client hiding a button.
[ApiController]
[Route("api/grades")]
public class GradesController : ControllerBase
{
    [HttpPost("publish")]
    [Authorize(Roles = "Teacher")]
    public IActionResult Publish()
    {
        return Ok(new { message = "Grades published." });
    }
}
