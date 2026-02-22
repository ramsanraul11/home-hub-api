namespace HomeHub.Infrastructure.Auth
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public IdentityService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Result<AuthUser>> RegisterAsync(string email, string password, CancellationToken ct)
        {
            try
            {
                var existing = await _userManager.FindByEmailAsync(email);
                if (existing is not null)
                    return Result<AuthUser>.Fail("auth.email_exists", "Email already registered.");

                var user = new AppUser { Email = email, UserName = email };

                var res = await _userManager.CreateAsync(user, password);
                if (!res.Succeeded)
                    return Result<AuthUser>.Fail("auth.register_failed", string.Join(" | ", res.Errors.Select(e => e.Description)));

                return Result<AuthUser>.Ok(new AuthUser(user.Id, user.Email!));
            }
            catch (Exception ex)
            {
                return Result<AuthUser>.Fail("auth.register_error", $"Registration exception: {ex.Message}");
            }
        }

        public async Task<Result<AuthUser>> ValidateCredentialsAsync(string email, string password, CancellationToken ct)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return Result<AuthUser>.Fail("auth.invalid_credentials", "Invalid credentials.");

                var ok = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
                if (!ok.Succeeded)
                    return Result<AuthUser>.Fail("auth.invalid_credentials", "Invalid credentials.");

                return Result<AuthUser>.Ok(new AuthUser(user.Id, user.Email!));
            }
            catch (Exception ex)
            {
                return Result<AuthUser>.Fail("auth.invalid_credentials_error", $"Invalid credentials exception: {ex.Message}");
            }
        }
    }
}
