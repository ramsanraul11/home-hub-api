namespace HomeHub.Infrastructure.Auth.Config
{
    public sealed class RefreshTokenConfig : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> b)
        {
            b.ToTable("RefreshToken");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.TokenHash).HasMaxLength(128).IsRequired(); // SHA256 hex = 64 chars, dejamos margen
            b.Property(x => x.ExpiresAtUtc).IsRequired();
            b.Property(x => x.CreatedAtUtc).IsRequired();

            b.HasIndex(x => x.TokenHash).IsUnique();
            b.HasIndex(x => new { x.UserId, x.ExpiresAtUtc });
            b.HasIndex(x => new { x.UserId, x.IsRevoked });

            b.HasOne<RefreshTokenEntity>()
                .WithMany()
                .HasForeignKey(x => x.ReplacedByTokenId)
                .OnDelete(DeleteBehavior.SetNull);

            // FK a Identity (AspNetUsers) sin navegar
            b.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
