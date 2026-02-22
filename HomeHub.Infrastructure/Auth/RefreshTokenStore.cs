using static HomeHub.Application.Auth.Ports.IRefreshTokenStore;

namespace HomeHub.Infrastructure.Auth
{
    public sealed class RefreshTokenStore : IRefreshTokenStore
    {
        private readonly AppDbContext _db;

        public RefreshTokenStore(AppDbContext db) => _db = db;

        public async Task<Result<NewRefreshToken>> CreateAsync(Guid userId, DateTime expiresAtUtc, string? userAgent, string? ip, CancellationToken ct)
        {
            var token = GenerateSecureToken();
            var hash = Sha256Hex(token);

            // extremadamente raro, pero por seguridad:
            var exists = await _db.RefreshTokens.AnyAsync(x => x.TokenHash == hash, ct);
            if (exists) return Result<NewRefreshToken>.Fail("auth.refresh_collision", "Please retry.");

            var entity = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = hash,
                ExpiresAtUtc = expiresAtUtc,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedByIp = ip,
                UserAgent = userAgent,
                IsRevoked = false
            };

            _db.RefreshTokens.Add(entity);
            await _db.SaveChangesAsync(ct);

            return Result<NewRefreshToken>.Ok(new NewRefreshToken(token, expiresAtUtc));
        }
        public async Task<Result<RefreshTokenRecord>> GetByTokenAsync(string refreshToken, CancellationToken ct)
        {
            var hash = Sha256Hex(refreshToken);

            var rt = await _db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
            if (rt is null) return Result<RefreshTokenRecord>.Fail("auth.refresh_not_found", "Invalid refresh token.");

            return Result<RefreshTokenRecord>.Ok(new RefreshTokenRecord(
                rt.Id, rt.UserId, rt.ExpiresAtUtc, rt.IsRevoked, rt.RevokedAtUtc, rt.ReplacedByTokenId
            ));
        }
        public async Task<Result<RotatedRefreshToken>> RotateAsync(string refreshToken, DateTime newExpiresAtUtc, string? userAgent, string? ip, CancellationToken ct)
        {
            var hash = Sha256Hex(refreshToken);

            var current = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
            if (current is null) return Result<RotatedRefreshToken>.Fail("auth.refresh_not_found", "Invalid refresh token.");

            if (current.IsRevoked) return Result<RotatedRefreshToken>.Fail("auth.refresh_revoked", "Refresh token revoked.");
            if (current.ExpiresAtUtc <= DateTime.UtcNow) return Result<RotatedRefreshToken>.Fail("auth.refresh_expired", "Refresh token expired.");
            if (current.ReplacedByTokenId is not null) return Result<RotatedRefreshToken>.Fail("auth.refresh_used", "Refresh token already used.");

            // crear nuevo token
            var newToken = GenerateSecureToken();
            var newHash = Sha256Hex(newToken);

            var next = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                UserId = current.UserId,
                TokenHash = newHash,
                ExpiresAtUtc = newExpiresAtUtc,
                CreatedAtUtc = DateTime.UtcNow,
                CreatedByIp = ip,
                UserAgent = userAgent,
                IsRevoked = false
            };

            // revocar el actual y enlazar
            current.IsRevoked = true;
            current.RevokedAtUtc = DateTime.UtcNow;
            current.RevokeReason = "rotated";
            current.RevokedByIp = ip;
            current.ReplacedByTokenId = next.Id;

            _db.RefreshTokens.Add(next);
            await _db.SaveChangesAsync(ct);

            return Result<RotatedRefreshToken>.Ok(new RotatedRefreshToken(
                UserId: current.UserId,
                Token: newToken,
                ExpiresAtUtc: newExpiresAtUtc
            ));
        }
        public async Task<Result> RevokeAsync(string refreshToken, string reason, string? ip, CancellationToken ct)
        {
            var hash = Sha256Hex(refreshToken);

            var current = await _db.RefreshTokens.FirstOrDefaultAsync(x => x.TokenHash == hash, ct);
            if (current is null) return Result.Fail("auth.refresh_not_found", "Invalid refresh token.");

            if (current.IsRevoked) return Result.Ok();

            current.IsRevoked = true;
            current.RevokedAtUtc = DateTime.UtcNow;
            current.RevokeReason = reason;
            current.RevokedByIp = ip;

            await _db.SaveChangesAsync(ct);
            return Result.Ok();
        }
        public async Task<Result> RevokeAllForUserAsync(Guid userId, string reason, CancellationToken ct)
        {
            var tokens = await _db.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ToListAsync(ct);

            foreach (var t in tokens)
            {
                t.IsRevoked = true;
                t.RevokedAtUtc = DateTime.UtcNow;
                t.RevokeReason = reason;
            }

            await _db.SaveChangesAsync(ct);
            return Result.Ok();
        }

        private static string GenerateSecureToken()
        {
            // 32 bytes => 256 bits
            var bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }

        private static string Sha256Hex(string input)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes); // uppercase hex
        }
    }
}
