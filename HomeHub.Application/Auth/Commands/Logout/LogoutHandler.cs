namespace HomeHub.Application.Auth.Commands.Logout
{
    public sealed class LogoutHandler
    {
        private readonly IRefreshTokenStore _refresh;
        public LogoutHandler(IRefreshTokenStore refresh) => _refresh = refresh;

        public Task<Result> Handle(LogoutCommand cmd, string? ip, CancellationToken ct)
            => _refresh.RevokeAsync(cmd.RefreshToken, "logout", ip, ct);
    }
}
