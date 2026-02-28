namespace HomeHub.Application.Shopping.Dtos
{
    public sealed record ShoppingListDto(
        Guid Id,
        Guid HouseholdId,
        string Name,
        bool IsArchived,
        DateTime CreatedAtUtc,
        Guid CreatedByUserId
    );

    public sealed record ShoppingListItemDto(
        Guid Id,
        Guid HouseholdId,
        Guid ShoppingListId,
        string Name,
        decimal Quantity,
        string? Notes,
        bool IsBought,
        DateTime? BoughtAtUtc,
        Guid? BoughtByUserId,
        DateTime CreatedAtUtc,
        Guid CreatedByUserId
    );
}