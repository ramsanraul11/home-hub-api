namespace HomeHub.Domain.Inventory
{
    public sealed class InventoryCategory
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }
        public string Name { get; private set; } = default!;
        public DateTime CreatedAtUtc { get; private set; }
        public Guid CreatedByUserId { get; private set; }

        private InventoryCategory() { }

        private InventoryCategory(Guid id, Guid householdId, string name, Guid createdByUserId)
        {
            Id = id;
            HouseholdId = householdId;
            Name = name;
            CreatedByUserId = createdByUserId;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static InventoryCategory Create(Guid householdId, string name, Guid createdByUserId)
            => new(Guid.NewGuid(), householdId, name.Trim(), createdByUserId);

        public void Rename(string name) => Name = name.Trim();
    }
}
