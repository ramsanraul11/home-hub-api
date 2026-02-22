namespace HomeHub.Application.Auth.Commands.Refresh
{
    public sealed class RefreshHandler
    {
        private readonly ITokenService _tokens;
        private readonly IRefreshTokenStore _refresh;

        public RefreshHandler(ITokenService tokens, IRefreshTokenStore refresh)
            => (_tokens, _refresh) = (tokens, refresh);

        public async Task<Result<AuthResponse>> Handle(RefreshCommand cmd, string? userAgent, string? ip, CancellationToken ct)
        {
            var rotated = await _refresh.RotateAsync(cmd.RefreshToken, DateTime.UtcNow.AddDays(30), userAgent, ip, ct);
            if (!rotated.IsSuccess) return Result<AuthResponse>.Fail(rotated.Error!.Code, rotated.Error!.Message);

            var user = new AuthUser(rotated.Value!.UserId, Email: ""); // email no es necesaria para claims básicas
            var access = await _tokens.CreateAccessTokenAsync(user, ct);

            return Result<AuthResponse>.Ok(new AuthResponse(
                AccessToken: access,
                RefreshToken: rotated.Value.Token,
                RefreshTokenExpiresAtUtc: rotated.Value.ExpiresAtUtc
            ));
        }
    }
}
