namespace HomeHub.Infrastructure.Shopping.Config
{
    public sealed class ShoppingListConfig : IEntityTypeConfiguration<ShoppingList>
    {
        public void Configure(EntityTypeBuilder<ShoppingList> b)
        {
            b.ToTable("ShoppingList");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.IsArchived).IsRequired();

            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.Property(x => x.CreatedByUserId).IsRequired();

            b.HasIndex(x => new { x.HouseholdId, x.IsArchived });
            b.HasIndex(x => new { x.HouseholdId, x.Name });
        }
    }
}