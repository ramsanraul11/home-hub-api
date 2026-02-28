namespace HomeHub.Domain.Shopping
{
    public sealed class ShoppingList : Entity
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }
        public string Name { get; private set; } = default!;
        public bool IsArchived { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }
        public Guid CreatedByUserId { get; private set; }

        private ShoppingList() { }

        private ShoppingList(Guid id, Guid householdId, string name, Guid createdByUserId)
        {
            Id = id;
            HouseholdId = householdId;
            Name = name.Trim();
            CreatedByUserId = createdByUserId;
            CreatedAtUtc = DateTime.UtcNow;
            IsArchived = false;
        }

        public static ShoppingList Create(Guid householdId, string name, Guid userId)
            => new(Guid.NewGuid(), householdId, name, userId);

        public void Archive() => IsArchived = true;
        public void Rename(string newName) => Name = newName.Trim();
    }
}

