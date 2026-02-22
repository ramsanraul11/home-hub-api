namespace HomeHub.Infrastructure.Households.Config
{
    public sealed class HouseholdConfig : IEntityTypeConfiguration<Household>
    {
        public void Configure(EntityTypeBuilder<Household> b)
        {
            b.ToTable("Household");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.Property(x => x.CreatedByUserId).IsRequired();

            b.HasIndex(x => x.CreatedAtUtc);
        }
    }
}
