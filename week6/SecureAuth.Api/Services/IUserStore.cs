using SecureAuth.Api.Models;

namespace SecureAuth.Api.Services;

public interface IUserStore
{
    User? FindByUsername(string username);
}
