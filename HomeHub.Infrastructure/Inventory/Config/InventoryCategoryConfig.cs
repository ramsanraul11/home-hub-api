namespace HomeHub.Infrastructure.Inventory.Config
{
    public sealed class InventoryCategoryConfig : IEntityTypeConfiguration<InventoryCategory>
    {
        public void Configure(EntityTypeBuilder<InventoryCategory> b)
        {
            b.ToTable("InventoryCategory");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();

            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.Property(x => x.CreatedByUserId).IsRequired();

            b.HasIndex(x => new { x.HouseholdId, x.Name }).IsUnique();
            b.HasIndex(x => x.HouseholdId);
        }
    }
}