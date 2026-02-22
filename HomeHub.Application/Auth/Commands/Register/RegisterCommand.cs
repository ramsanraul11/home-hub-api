namespace HomeHub.Application.Auth.Commands.Register
{
    public sealed record RegisterCommand(string Email, string Password);
    public sealed record AuthResponse(string AccessToken);
}
