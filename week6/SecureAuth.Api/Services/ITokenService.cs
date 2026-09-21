using SecureAuth.Api.Models;

namespace SecureAuth.Api.Services;

public interface ITokenService
{
    string IssueToken(User user);
}
