namespace HomeHub.Domain.Shopping
{
    public sealed class ShoppingListItem
    {
        public Guid Id { get; private set; }
        public Guid ShoppingListId { get; private set; }
        public Guid HouseholdId { get; private set; }

        public string Name { get; private set; } = default!;
        public decimal Quantity { get; private set; }
        public string? Notes { get; private set; }

        public bool IsBought { get; private set; }
        public DateTime? BoughtAtUtc { get; private set; }
        public Guid? BoughtByUserId { get; private set; }

        public DateTime CreatedAtUtc { get; private set; }
        public Guid CreatedByUserId { get; private set; }

        private ShoppingListItem() { }

        private ShoppingListItem(Guid id, Guid householdId, Guid listId, string name, decimal quantity, string? notes, Guid userId)
        {
            Id = id;
            HouseholdId = householdId;
            ShoppingListId = listId;
            Name = name.Trim();
            Quantity = quantity;
            Notes = notes;
            CreatedByUserId = userId;
            CreatedAtUtc = DateTime.UtcNow;
            IsBought = false;
        }

        public static ShoppingListItem Create(Guid householdId, Guid listId, string name, decimal quantity, string? notes, Guid userId)
            => new(Guid.NewGuid(), householdId, listId, name, quantity, notes, userId);

        public void MarkBought(Guid userId)
        {
            IsBought = true;
            BoughtByUserId = userId;
            BoughtAtUtc = DateTime.UtcNow;
        }

        public void UnmarkBought()
        {
            IsBought = false;
            BoughtByUserId = null;
            BoughtAtUtc = null;
        }
    }
}
