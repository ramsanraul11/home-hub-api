namespace HomeHub.Api.Security
{
    public static class CurrentUser
    {
        public static Guid GetUserId(ClaimsPrincipal user)
        {
            var raw = user.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? user.FindFirstValue("sub");

            if (raw is null || !Guid.TryParse(raw, out var id))
                throw new InvalidOperationException("User id claim missing/invalid.");

            return id;
        }
    }
}
