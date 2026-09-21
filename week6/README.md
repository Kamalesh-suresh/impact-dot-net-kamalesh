# Week 6 — File Handling, Encryption & Hashing, Auth Fundamentals

**SecureFileVault**: a password → PBKDF2 → AES-GCM streaming file vault
(chunked, tamper-rejected), plus every crypto fundamental it's built on
(file streaming, AES-CBC, hashing/HMAC, constant-time comparison, RSA
concept) — and **SecureAuth.Api**, a minimal JWT login slice with a
role-gated endpoint (Task 6.15).

## Layer responsibilities

- **SecureFileVault.Core** holds every crypto/file-handling primitive
  (Tasks 6.1–6.13, 6.15's `PasswordHasher`) — the only project unit-tested
  directly.
- **SecureFileVault.App** is the runnable console tool: a numbered menu
  covering each task, plus real `encrypt`/`decrypt` vault commands.
- **SecureAuth.Api** is Task 6.15's login slice — an ASP.NET Core Web API
  that issues JWTs and enforces a role check server-side.
- **SecureFileVault.Tests** is the xUnit project: unit tests against Core's
  crypto logic, plus `AuthApiTests`, which drives the Api through a real
  HTTP pipeline (`WebApplicationFactory`) to prove `[Authorize(Roles =
  "Teacher")]` is actually enforced.

## Project layout

```
SecureFileVault.Core/
  FileHandling/  ChunkedFileCopier                     (6.1)
  Fundamentals/  CodecDemo                              (6.2)
  Symmetric/     AesCbcService                          (6.4-6.6)
  Kdf/           Pbkdf2KeyDerivation                    (6.7)
  Aead/          AesGcmBufferService, ChunkedGcmVaultService  (6.8, 6.9 + the vault itself)
  Streaming/     CryptoStreamFileCipher                 (6.12, literal CryptoStream chaining)
  Hashing/       FileHasher, ConstantTimeComparer        (6.10, 6.11)
  Rsa/           RsaDemo                                 (6.13, concept only)
  Auth/          PasswordHasher                          (6.15's storage half, shared with the Api)
SecureFileVault.App/     console menu covering every task above + vault encrypt/decrypt
SecureAuth.Api/          POST api/auth/login (JWT + role claim), POST api/grades/publish [Authorize(Roles="Teacher")]
SecureFileVault.Tests/   xUnit + Moq + coverlet, covers Core + drives SecureAuth.Api over real HTTP
CRYPTO-NOTES.md          Day 1/3/5 write-ups (6.2/6.3, 6.7/6.9, 6.13/6.14)
```

See [CRYPTO-NOTES.md](CRYPTO-NOTES.md) for *why* things are built this way
— in particular, why the vault uses chunked AES-GCM rather than threading
`AesGcm` through `CryptoStream` (it can't be — `AesGcm` doesn't implement
`ICryptoTransform`, and that's by design, not an oversight).

## Run

### SecureFileVault (console)
```bash
dotnet run --project SecureFileVault.App
```
Opens a numbered menu (file handling, encoding/hashing, AES-CBC, PBKDF2,
AES-GCM + tamper demo, hashing/HMAC/constant-time, a 100 MB+ CryptoStream
round trip, an RSA demo, and the vault itself — automated demo or
interactive encrypt/decrypt).

Or use the vault directly, no menu:
```bash
dotnet run --project SecureFileVault.App -- encrypt <source> <destination.sfv> <password>
dotnet run --project SecureFileVault.App -- decrypt <destination.sfv> <restored-path> <password>
```

### SecureAuth.Api
```bash
dotnet run --project SecureAuth.Api
```
Two users are seeded in memory: `mr_rao` / `teacher-pass-123` (role
Teacher) and `alice` / `student-pass-123` (role Student). See
[SecureAuth.Api/SecureAuth.Api.http](SecureAuth.Api/SecureAuth.Api.http)
for ready-to-run login + role-check requests, or:
```bash
curl -X POST http://localhost:5080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"mr_rao","password":"teacher-pass-123"}'

curl -X POST http://localhost:5080/api/grades/publish \
  -H "Authorization: Bearer <token from above>"
```
A Teacher token gets 200; the same request with a Student token gets 403;
no token gets 401. Swagger UI (Development only) has a Bearer "Authorize"
button pre-wired for the same flow.

## Test + coverage (≥80%)
```bash
dotnet test SecureFileVault.sln --collect:"XPlat Code Coverage"
```
40 tests, ~97% line coverage across `SecureFileVault.Core` and
`SecureAuth.Api`. Per the curriculum's testing focus: PBKDF2 determinism,
AES-GCM/vault round trips *and* tamper rejection, constant-time compare,
password hash accept/reject, and the Student-403/Teacher-200 role split —
each has a dedicated failure-path test, not just the happy path.
