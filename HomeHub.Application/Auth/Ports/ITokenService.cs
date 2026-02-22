namespace HomeHub.Application.Auth.Ports
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(AuthUser user, CancellationToken ct);
    }
}
