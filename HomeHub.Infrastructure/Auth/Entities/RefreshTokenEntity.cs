namespace HomeHub.Infrastructure.Auth.Entities
{
    public sealed class RefreshTokenEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } // FK a AspNetUsers

        public string TokenHash { get; set; } = default!;
        public DateTime ExpiresAtUtc { get; set; }

        public bool IsRevoked { get; set; }
        public DateTime? RevokedAtUtc { get; set; }
        public string? RevokeReason { get; set; }
        public string? RevokedByIp { get; set; }

        public Guid? ReplacedByTokenId { get; set; }
        public RefreshTokenEntity? ReplacedByToken { get; set; }

        public DateTime CreatedAtUtc { get; set; }
        public string? CreatedByIp { get; set; }
        public string? UserAgent { get; set; }
    }
}
