namespace HomeHub.Infrastructure.Shopping.Config
{
    public sealed class ShoppingListItemConfig : IEntityTypeConfiguration<ShoppingListItem>
    {
        public void Configure(EntityTypeBuilder<ShoppingListItem> b)
        {
            b.ToTable("ShoppingListItem");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.ShoppingListId).IsRequired();

            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.Property(x => x.Quantity).HasColumnType("numeric(18,2)").IsRequired();
            b.Property(x => x.Notes).HasMaxLength(1000);

            b.Property(x => x.IsBought).IsRequired();
            b.Property(x => x.BoughtAtUtc);
            b.Property(x => x.BoughtByUserId);

            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.Property(x => x.CreatedByUserId).IsRequired();

            b.HasIndex(x => new { x.HouseholdId, x.ShoppingListId, x.IsBought });
            b.HasIndex(x => new { x.ShoppingListId, x.CreatedAtUtc });

            b.HasOne<ShoppingList>()
                .WithMany()
                .HasForeignKey(x => x.ShoppingListId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
