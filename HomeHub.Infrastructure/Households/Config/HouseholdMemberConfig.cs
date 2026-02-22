namespace HomeHub.Infrastructure.Households.Config
{
    public sealed class HouseholdMemberConfig : IEntityTypeConfiguration<HouseholdMember>
    {
        public void Configure(EntityTypeBuilder<HouseholdMember> b)
        {
            b.ToTable("HouseholdMember");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.Role).IsRequired();
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.JoinedAtUtc).IsRequired();

            b.HasIndex(x => new { x.HouseholdId, x.UserId }).IsUnique();
            b.HasIndex(x => new { x.HouseholdId, x.Status });

            b.HasOne<Household>()
                .WithMany()
                .HasForeignKey(x => x.HouseholdId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
