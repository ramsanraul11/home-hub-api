namespace HomeHub.Domain.Shopping.Events
{
    public sealed record ShoppingItemDeleted(
        Guid Id,
        DateTime OccurredAtUtc,
        Guid HouseholdId,
        Guid ListId,
        Guid ItemId,
        Guid ActorUserId
    ) : DomainEvent(Id, OccurredAtUtc);
}
