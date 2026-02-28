namespace HomeHub.Infrastructure.Inventory.Config
{
    public sealed class InventoryItemConfig : IEntityTypeConfiguration<InventoryItem>
    {
        public void Configure(EntityTypeBuilder<InventoryItem> b)
        {
            b.ToTable("InventoryItem");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.CategoryId);

            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Unit).IsRequired();

            b.Property(x => x.Quantity).HasColumnType("numeric(18,2)").IsRequired();
            b.Property(x => x.MinimumQuantity).HasColumnType("numeric(18,2)").IsRequired();

            b.Property(x => x.UpdatedAtUtc).IsRequired();
            b.Property(x => x.UpdatedByUserId).IsRequired();

            b.HasIndex(x => new { x.HouseholdId, x.CategoryId });
            b.HasIndex(x => new { x.HouseholdId, x.Name });

            b.HasOne<InventoryCategory>()
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}