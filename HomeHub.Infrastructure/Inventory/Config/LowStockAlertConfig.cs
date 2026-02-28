namespace HomeHub.Infrastructure.Inventory.Config
{
    public sealed class LowStockAlertConfig : IEntityTypeConfiguration<LowStockAlert>
    {
        public void Configure(EntityTypeBuilder<LowStockAlert> b)
        {
            b.ToTable("LowStockAlert");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.ItemId).IsRequired();
            b.Property(x => x.ItemName).HasMaxLength(200).IsRequired();
            b.Property(x => x.Quantity).HasColumnType("numeric(18,2)").IsRequired();
            b.Property(x => x.MinimumQuantity).HasColumnType("numeric(18,2)").IsRequired();
            b.Property(x => x.TriggeredAtUtc).IsRequired();

            b.Property(x => x.ResolvedAtUtc);
            b.Property(x => x.ResolvedByUserId);

            b.HasIndex(x => new { x.HouseholdId, x.ItemId });

            // Evita alertas duplicadas "activas" para el mismo item (Postgres partial index)
            b.HasIndex(x => new { x.HouseholdId, x.ItemId })
                .HasFilter("\"ResolvedAtUtc\" IS NULL")
                .IsUnique();
        }
    }
}