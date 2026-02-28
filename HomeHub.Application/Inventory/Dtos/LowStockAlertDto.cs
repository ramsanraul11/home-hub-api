namespace HomeHub.Application.Inventory.Dtos
{
    public sealed record LowStockAlertDto(
        Guid Id,
        Guid ItemId,
        string ItemName,
        decimal Quantity,
        decimal MinimumQuantity,
        DateTime TriggeredAtUtc,
        DateTime? ResolvedAtUtc,
        Guid? ResolvedByUserId
    );
}