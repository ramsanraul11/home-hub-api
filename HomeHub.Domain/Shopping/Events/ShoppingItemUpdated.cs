namespace HomeHub.Domain.Shopping.Events
{
    public sealed record ShoppingItemUpdated(
    Guid Id,
    DateTime OccurredAtUtc,
    Guid HouseholdId,
    Guid ListId,
    Guid ItemId,
    string Name,
    decimal Quantity,
    Guid ActorUserId
) : DomainEvent(Id, OccurredAtUtc);
}
