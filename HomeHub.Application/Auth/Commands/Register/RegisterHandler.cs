namespace HomeHub.Application.Auth.Commands.Register
{
    public sealed class RegisterHandler
    {
        private readonly IIdentityService _identity;
        private readonly ITokenService _tokens;

        public RegisterHandler(IIdentityService identity, ITokenService tokens)
        {
            _identity = identity;
            _tokens = tokens;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterCommand cmd, CancellationToken ct)
        {
            var reg = await _identity.RegisterAsync(cmd.Email, cmd.Password, ct);
            if (!reg.IsSuccess) return Result<AuthResponse>.Fail(reg.Error!.Code, reg.Error!.Message);

            var token = await _tokens.CreateAccessTokenAsync(reg.Value!, ct);
            return Result<AuthResponse>.Ok(new AuthResponse(token));
        }
    }
}
