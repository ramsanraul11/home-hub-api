using HomeHub.Domain.Inventory.Events;

namespace HomeHub.Domain.Inventory
{
    public enum InventoryUnit { Unit = 1, Kg = 2, L = 3, Pack = 4 }

    public sealed class InventoryItem : Entity
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }
        public Guid? CategoryId { get; private set; }

        public string Name { get; private set; } = default!;
        public InventoryUnit Unit { get; private set; }

        public decimal Quantity { get; private set; }
        public decimal MinimumQuantity { get; private set; }

        public DateTime UpdatedAtUtc { get; private set; }
        public Guid UpdatedByUserId { get; private set; }

        private InventoryItem() { }

        private InventoryItem(Guid id, Guid householdId, Guid? categoryId, string name, InventoryUnit unit, decimal quantity, decimal min, Guid userId)
        {
            Id = id;
            HouseholdId = householdId;
            CategoryId = categoryId;
            Name = name;
            Unit = unit;
            Quantity = quantity;
            MinimumQuantity = min;
            UpdatedByUserId = userId;
            UpdatedAtUtc = DateTime.UtcNow;
        }

        public static InventoryItem Create(Guid householdId, Guid? categoryId, string name, InventoryUnit unit, decimal quantity, decimal min, Guid userId)
            => new(Guid.NewGuid(), householdId, categoryId, name.Trim(), unit, quantity, min, userId);

        private void CheckLowStockTransition(bool wasLowStock)
        {
            var isLowStock = IsLowStock();
            if (!wasLowStock && isLowStock)
            {
                Raise(new LowStockTriggered(
                    DomainEvent.NewId(),
                    DateTime.UtcNow,
                    HouseholdId,
                    Id,
                    Name,
                    Quantity,
                    MinimumQuantity
                ));
            }
        }

        public void Update(string name, Guid? categoryId, InventoryUnit unit, decimal quantity, decimal min, Guid userId)
        {
            var wasLowStock = IsLowStock();
            Name = name.Trim();
            CategoryId = categoryId;
            Unit = unit;
            Quantity = quantity;
            MinimumQuantity = min;
            UpdatedByUserId = userId;
            UpdatedAtUtc = DateTime.UtcNow;
            CheckLowStockTransition(wasLowStock);
        }

        public void Add(decimal amount, Guid userId)
        {
            var wasLowStock = IsLowStock();
            Quantity += amount;
            UpdatedByUserId = userId;
            UpdatedAtUtc = DateTime.UtcNow;
            CheckLowStockTransition(wasLowStock);
        }

        public void Consume(decimal amount, Guid userId)
        {
            var wasLowStock = IsLowStock();
            Quantity = Math.Max(0, Quantity - amount);
            UpdatedByUserId = userId;
            UpdatedAtUtc = DateTime.UtcNow;
            CheckLowStockTransition(wasLowStock);
        }

        public bool IsLowStock() => Quantity <= MinimumQuantity;
    }
}
