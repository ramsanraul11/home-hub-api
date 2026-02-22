namespace HomeHub.Application.Auth.Commands.Login
{
    public sealed class LoginHandler
    {
        private readonly IIdentityService _identity;
        private readonly ITokenService _tokens;

        public LoginHandler(IIdentityService identity, ITokenService tokens)
        {
            _identity = identity;
            _tokens = tokens;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand cmd, CancellationToken ct)
        {
            var valid = await _identity.ValidateCredentialsAsync(cmd.Email, cmd.Password, ct);
            if (!valid.IsSuccess) return Result<AuthResponse>.Fail(valid.Error!.Code, valid.Error!.Message);

            var token = await _tokens.CreateAccessTokenAsync(valid.Value!, ct);
            return Result<AuthResponse>.Ok(new AuthResponse(token));
        }
    }
}
