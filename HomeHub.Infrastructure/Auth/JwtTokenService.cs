namespace HomeHub.Infrastructure.Auth
{
    public sealed class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public JwtTokenService(IConfiguration config) => _config = config;

        public Task<string> CreateAccessTokenAsync(AuthUser user, CancellationToken ct)
        {
            var jwt = _config.GetSection("Jwt");
            var issuer = jwt["Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer missing");
            var audience = jwt["Audience"] ?? throw new InvalidOperationException("Jwt:Audience missing");
            var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key missing");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
