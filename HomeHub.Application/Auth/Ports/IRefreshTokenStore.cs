namespace HomeHub.Application.Auth.Ports
{
    public sealed record NewRefreshToken(
        string Token,          // en claro, se devuelve al cliente
        DateTime ExpiresAtUtc
    );

    public sealed record RefreshTokenRecord(
        Guid Id,
        Guid UserId,
        DateTime ExpiresAtUtc,
        bool IsRevoked,
        DateTime? RevokedAtUtc,
        Guid? ReplacedByTokenId
    );

    public interface IRefreshTokenStore
    {
        Task<Result<NewRefreshToken>> CreateAsync(
            Guid userId,
            DateTime expiresAtUtc,
            string? userAgent,
            string? ip,
            CancellationToken ct);

        Task<Result<RefreshTokenRecord>> GetByTokenAsync(
            string refreshToken,
            CancellationToken ct);

        public sealed record RotatedRefreshToken(
            Guid UserId,
            string Token,
            DateTime ExpiresAtUtc
        );

        Task<Result<RotatedRefreshToken>> RotateAsync(
            string refreshToken,
            DateTime newExpiresAtUtc,
            string? userAgent,
            string? ip,
            CancellationToken ct);

        Task<Result> RevokeAsync(
            string refreshToken,
            string reason,
            string? ip,
            CancellationToken ct);

        Task<Result> RevokeAllForUserAsync(
            Guid userId,
            string reason,
            CancellationToken ct);
    }
}
