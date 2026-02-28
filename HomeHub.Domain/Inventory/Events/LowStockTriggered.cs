namespace HomeHub.Domain.Inventory.Events
{
    public sealed record LowStockTriggered(
        Guid EventId,
        DateTime OccurredAtUtc,
        Guid HouseholdId,
        Guid ItemId,
        string ItemName,
        decimal Quantity,
        decimal MinimumQuantity
    ) : DomainEvent(EventId, OccurredAtUtc);

}
