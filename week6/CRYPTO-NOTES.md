# Crypto Notes — Week 6

Write-ups for the three "understand, don't just build" tasks (6.2/6.3, 6.7/6.9,
6.13/6.14), each backed by a runnable proof in `SecureFileVault.App`/`.Core`.

## Day 1 — Encoding vs hashing vs encryption; cipher modes (6.2, 6.3)

**The one-line distinction** (proven in `CodecDemo`, `FundamentalsDemo`,
menu option 2):

| | Reversible? | Needs a secret? | Output shape |
|---|---|---|---|
| **Encoding** (Base64) | Yes, always | No | Same length family, no security property |
| **Hashing** (SHA-256) | No, one-way | No | Fixed length regardless of input |
| **Encryption** (AES/RSA) | Yes, but only with the key | Yes | Depends on mode/padding |

The mistake this task is guarding against: treating Base64 as "obfuscation"
or "security." It hides data from a casual glance at a URL or log line, not
from anyone who actually tries — `Convert.FromBase64String` undoes it in one
line, no key required. If the goal is "nobody without X can read this,"
Base64 is never X.

**Cipher modes — what each provides, why ECB leaks patterns:**

- **ECB** encrypts each block independently with the same key. Identical
  plaintext blocks → identical ciphertext blocks, always. Structure in the
  input (repeated bytes, a flat-color region of an image, a mostly-empty
  file) shows up as repeated ciphertext blocks — the textbook example is
  encrypting a bitmap in ECB and still being able to see the outline of the
  image in the ciphertext. Never use it.
- **CBC** XORs each plaintext block with the *previous ciphertext block*
  before encrypting, so identical plaintext blocks stop producing identical
  ciphertext (this project's `AesCbcService` demo, Task 6.5, proves that
  directly). Still no authentication — Task 6.6 shows decrypting with the
  wrong key either throws from broken padding or, worse, occasionally
  produces silent garbage with no error at all.
- **CTR** turns the block cipher into a stream cipher: it encrypts an
  incrementing counter and XORs the result with the plaintext. Fast,
  parallelizable, no padding — but, like CBC, gives confidentiality only.
- **GCM** is CTR mode plus a built-in authentication tag computed over the
  ciphertext. It's the only one of the four that detects tampering on its
  own: `AesGcmBufferService`/`ChunkedGcmVaultService` (Tasks 6.8, 6.9) throw
  on any single-bit change instead of silently returning wrong data.

## Day 3 — PBKDF2, AES-GCM tamper rejection, and why the vault is chunked (6.7, 6.9)

**Why salt + iterations (`Pbkdf2KeyDerivation`, Task 6.7):** a password
alone is a bad encryption key — it's low-entropy and attackers keep
precomputed tables (rainbow tables) of hash-to-password mappings for common
passwords. Salt defeats precomputation: it's random per secret, not itself
secret, and just needs to be unique so two users with the same password
never derive the same key, and no precomputed table can cover every
possible salt. Iterations (100,000 rounds of SHA-256 here) exist purely to
make each *guess* expensive: an attacker who steals a salt+hash pair still
has to redo the full derivation per candidate password, turning an
offline brute-force from cheap to expensive.

**GCM tamper rejection vs CBC's silent corruption (Task 6.9):** `AesGcm`
computes a 16-byte authentication tag over the ciphertext at encryption
time, and `Decrypt` verifies that tag *before* returning any plaintext.
Flip one bit anywhere in the ciphertext, the tag, or the nonce, and
`AesGcm.Decrypt` throws `AuthenticationTagMismatchException` (a
`CryptographicException`) instead of returning altered data. Contrast with
CBC (Task 6.6): a tampered CBC ciphertext usually breaks padding and
throws, but there's roughly a 1-in-256 chance the tampered bytes happen to
unpad cleanly — in which case CBC decrypts to *wrong plaintext with no
error at all*. GCM has no equivalent gap: the tag check is unconditional.

**Why the vault chunks the file instead of one `AesGcm.Encrypt` call:**
.NET's `AesGcm` doesn't implement `ICryptoTransform`, so — unlike `Aes`
(CBC) — it cannot be threaded through `CryptoStream`. That's deliberate,
not a missing feature: GCM's security proof depends on verifying the
*entire* tag before releasing *any* plaintext. A true single-tag "streaming
GCM" over an arbitrarily large file would have to either buffer the whole
file (defeating the point of streaming) or hand back unauthenticated
plaintext incrementally (defeating the point of GCM). `ChunkedGcmVaultService`
takes the standard real-world answer instead — the same shape TLS records,
`age`, and libsodium's `secretstream` all use: split the file into
fixed-size chunks, give each one its own nonce (an 8-byte random prefix
plus a 4-byte chunk counter, so nonces never repeat under one key) and its
own tag, so at most one chunk is ever held in memory and every chunk is
independently authenticated. A tampered byte anywhere makes exactly the
chunk it's in fail to decrypt — the whole file is rejected, as required.

## Day 5 — RSA concept, hybrid encryption, authn/authz, sessions vs tokens (6.13, 6.14)

**RSA (`RsaDemo`, concept only — nothing else in this project uses RSA):**
a key *pair* replaces a shared secret. The public key encrypts (anyone can
send you a secret) and verifies signatures; the private key decrypts (only
you can read what was sent to you) and signs (only you can produce a
signature others can verify with your public key) — encryption and signing
use the same pair, in opposite directions. RSA also has a hard size limit:
with OAEP-SHA256 padding, max plaintext is `keySizeBytes - 2*hashSizeBytes
- 2` — for a 2048-bit key that's 256 − 64 − 2 = **190 bytes**
(`RsaDemo.MaxOaepSha256PlaintextBytes`). That's nowhere near enough for a
file, which is why real protocols never RSA-encrypt bulk data directly:
**hybrid encryption** generates a random AES key, encrypts the actual data
with AES (no size limit, fast), then RSA-encrypts only that small AES key
with the recipient's public key. This is exactly what TLS does during a
handshake — the "encrypt everything with RSA" approach doesn't scale past
a couple hundred bytes.

**Authentication vs authorization:** authentication answers "who are you?"
— in `SecureAuth.Api` this is the `POST /api/auth/login` step: the server
verifies a password against a stored PBKDF2 hash and, if it matches, issues
a signed JWT asserting an identity (and a role). Authorization answers
"what are you allowed to do, now that I know who you are?" — that's
`[Authorize(Roles = "Teacher")]` on `GradesController.Publish`: a valid
token (successful authentication) from a *Student* still gets rejected
(403) because authorization is a separate check, evaluated per-endpoint,
after identity is already established. A logged-in user and a permitted
user are not the same question.

**Session/cookie vs token:** a session/cookie flow has the server keep
state — on login it creates a session record (in memory, in a database, in
Redis) and gives the browser an opaque cookie that's just a lookup key;
every request the server looks the session up server-side to know who's
asking. A token flow (what `SecureAuth.Api` does) has the server issue a
signed, **self-contained** credential — the JWT already carries the
username and role claims inside it, so the server checks the signature and
trusts what's inside without a database round-trip or server-side session
state per request. That statelessness is why tokens fit APIs/microservices
well (no shared session store needed across instances) but it also means a
token can't be individually revoked before it expires without extra
infrastructure (a blocklist, short expiries) — a cost cookie/session-based
auth doesn't have, since deleting the server-side session record is
immediate.
