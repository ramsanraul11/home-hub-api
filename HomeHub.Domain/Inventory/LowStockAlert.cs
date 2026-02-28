namespace HomeHub.Domain.Inventory
{
    public sealed class LowStockAlert
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }
        public Guid ItemId { get; private set; }
        public string ItemName { get; private set; } = default!;
        public decimal Quantity { get; private set; }
        public decimal MinimumQuantity { get; private set; }

        public DateTime TriggeredAtUtc { get; private set; }

        public DateTime? ResolvedAtUtc { get; private set; }
        public Guid? ResolvedByUserId { get; private set; }

        private LowStockAlert() { } // EF

        private LowStockAlert(Guid id, Guid householdId, Guid itemId, string itemName, decimal quantity, decimal min, DateTime triggeredAtUtc)
        {
            Id = id;
            HouseholdId = householdId;
            ItemId = itemId;
            ItemName = itemName;
            Quantity = quantity;
            MinimumQuantity = min;
            TriggeredAtUtc = triggeredAtUtc;
        }

        public static LowStockAlert Create(Guid householdId, Guid itemId, string itemName, decimal quantity, decimal min, DateTime triggeredAtUtc)
            => new(Guid.NewGuid(), householdId, itemId, itemName, quantity, min, triggeredAtUtc);

        public void Resolve(Guid userId)
        {
            if (ResolvedAtUtc is not null) return;
            ResolvedAtUtc = DateTime.UtcNow;
            ResolvedByUserId = userId;
        }
    }
}