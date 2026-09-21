using Microsoft.AspNetCore.Mvc;
using SecureAuth.Api.Dtos;
using SecureAuth.Api.Services;
using SecureFileVault.Core.Auth;

namespace SecureAuth.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserStore _users;
    private readonly ITokenService _tokens;

    public AuthController(IUserStore users, ITokenService tokens)
    {
        _users = users;
        _tokens = tokens;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login(LoginRequestDto request)
    {
        var user = _users.FindByUsername(request.Username);
        if (user is null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        var token = _tokens.IssueToken(user);
        return Ok(new LoginResponseDto(token, user.Username, user.Role));
    }
}
