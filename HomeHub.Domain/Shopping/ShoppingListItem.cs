using HomeHub.Domain.Shopping.Events;

namespace HomeHub.Domain.Shopping
{
    public sealed class ShoppingListItem : Entity
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

            Raise(new ShoppingItemBoughtStateChanged(
                Id: Guid.NewGuid(),
                OccurredAtUtc: DateTime.UtcNow,
                HouseholdId: HouseholdId,
                ListId: ShoppingListId,
                ItemId: Id,
                IsBought: true,
                ActorUserId: userId
            )); 
        }

        public void UnmarkBought(Guid actorUserId)
        {
            IsBought = false;
            BoughtByUserId = null;
            BoughtAtUtc = null;

            Raise(new ShoppingItemBoughtStateChanged(
                Id: Guid.NewGuid(),
                OccurredAtUtc: DateTime.UtcNow,
                HouseholdId: HouseholdId,
                ListId: ShoppingListId,
                ItemId: Id,
                IsBought: false,
                ActorUserId: actorUserId
            ));
        }
        public void Update(string name, decimal quantity, string? notes, Guid actorUserId)
        {
            Name = name.Trim();
            Quantity = quantity;
            Notes = notes;

            Raise(new ShoppingItemUpdated(
                Id: Guid.NewGuid(),
                OccurredAtUtc: DateTime.UtcNow,
                HouseholdId: HouseholdId,
                ListId: ShoppingListId,
                ItemId: Id,
                Name: Name,
                Quantity: Quantity,
                ActorUserId: actorUserId
            ));
        }

        public void RaiseAdded(Guid actorUserId)
        {
            Raise(new ShoppingItemAdded(
                Id: Guid.NewGuid(),
                OccurredAtUtc: DateTime.UtcNow,
                HouseholdId: HouseholdId,
                ListId: ShoppingListId,
                ItemId: Id,
                Name: Name,
                Quantity: Quantity,
                ActorUserId: actorUserId
            ));
        }

        public void RaiseDeleted(Guid actorUserId)
        {
            Raise(new ShoppingItemDeleted(
                Id: Guid.NewGuid(),
                OccurredAtUtc: DateTime.UtcNow,
                HouseholdId: HouseholdId,
                ListId: ShoppingListId,
                ItemId: Id,
                ActorUserId: actorUserId
            ));
        }
    }
}
