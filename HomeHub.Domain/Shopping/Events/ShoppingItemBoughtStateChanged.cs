namespace HomeHub.Domain.Shopping.Events
{
    public sealed record ShoppingItemBoughtStateChanged(
        Guid Id,
        DateTime OccurredAtUtc,
        Guid HouseholdId,
        Guid ListId,
        Guid ItemId,
        bool IsBought,
        Guid ActorUserId
    ) : DomainEvent(Id, OccurredAtUtc);
}
