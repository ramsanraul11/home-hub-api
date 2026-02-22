namespace HomeHub.Application.Auth.Ports
{
    public record AuthUser(Guid Id, string Email);

    public interface IIdentityService
    {
        Task<Result<AuthUser>> RegisterAsync(string email, string password, CancellationToken ct);
        Task<Result<AuthUser>> ValidateCredentialsAsync(string email, string password, CancellationToken ct);
    }
}
