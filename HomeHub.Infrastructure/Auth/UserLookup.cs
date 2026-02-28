namespace HomeHub.Infrastructure.Auth
{
    public sealed class UserLookup : IUserLookup
    {
        private readonly UserManager<AppUser> _userManager;
        public UserLookup(UserManager<AppUser> userManager) => _userManager = userManager;

        public async Task<Result<UserLookupDto>> FindByEmailAsync(string email, CancellationToken ct)
        {
            var normalized = (email ?? "").Trim();
            if (string.IsNullOrWhiteSpace(normalized))
                return Result<UserLookupDto>.Fail("user.email_invalid", "Email is required.");

            var user = await _userManager.FindByEmailAsync(normalized);
            if (user is null)
                return Result<UserLookupDto>.Fail("user.not_found", "User not found.");

            return Result<UserLookupDto>.Ok(new UserLookupDto(user.Id, user.Email!));
        }
    }
}
