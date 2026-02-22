namespace HomeHub.Application.Auth.Ports
{
    public sealed record UserLookupDto(Guid Id, string Email);

    public interface IUserLookup
    {
        Task<Result<UserLookupDto>> FindByEmailAsync(string email, CancellationToken ct);
    }
}
