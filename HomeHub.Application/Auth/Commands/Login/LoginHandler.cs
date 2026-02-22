namespace HomeHub.Application.Auth.Commands.Login
{
    public sealed class LoginHandler
    {
        private readonly IIdentityService _identity;
        private readonly ITokenService _tokens;
        private readonly IRefreshTokenStore _refresh;

        public LoginHandler(IIdentityService identity, ITokenService tokens, IRefreshTokenStore refresh)
            => (_identity, _tokens, _refresh) = (identity, tokens, refresh);

        public async Task<Result<AuthResponse>> Handle(LoginCommand cmd, string? userAgent, string? ip, CancellationToken ct)
        {
            var valid = await _identity.ValidateCredentialsAsync(cmd.Email, cmd.Password, ct);
            if (!valid.IsSuccess) return Result<AuthResponse>.Fail(valid.Error!.Code, valid.Error!.Message);

            var access = await _tokens.CreateAccessTokenAsync(valid.Value!, ct);

            var refreshExp = DateTime.UtcNow.AddDays(30);
            var refreshRes = await _refresh.CreateAsync(valid.Value!.Id, refreshExp, userAgent, ip, ct);
            if (!refreshRes.IsSuccess) return Result<AuthResponse>.Fail(refreshRes.Error!.Code, refreshRes.Error!.Message);

            return Result<AuthResponse>.Ok(new AuthResponse(access, refreshRes.Value!.Token, refreshRes.Value!.ExpiresAtUtc));
        }
    }
}
