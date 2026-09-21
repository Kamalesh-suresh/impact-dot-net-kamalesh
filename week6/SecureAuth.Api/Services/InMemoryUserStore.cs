using SecureFileVault.Core.Auth;
using SecureAuth.Api.Models;

namespace SecureAuth.Api.Services;

// Task 6.15: two seeded users, one per role, so the login+role-check flow
// has both a 200 and a 403 case to demonstrate. Passwords are hashed
// through Core's PasswordHasher (Task 6.7's PBKDF2) — the plaintext
// passwords below exist only as seed input, never stored.
public class InMemoryUserStore : IUserStore
{
    private readonly List<User> _users;

    public InMemoryUserStore()
    {
        _users =
        [
            new User { Username = "alice", PasswordHash = PasswordHasher.Hash("student-pass-123"), Role = "Student" },
            new User { Username = "mr_rao", PasswordHash = PasswordHasher.Hash("teacher-pass-123"), Role = "Teacher" }
        ];
    }

    public User? FindByUsername(string username) =>
        _users.FirstOrDefault(u => string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
}
