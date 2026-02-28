namespace HomeHub.Application.Inventory.Commands.CreateItem
{
    public sealed record CreateItemCommand(
        string Name,
        Guid? CategoryId,
        InventoryUnit Unit,
        decimal Quantity,
        decimal MinimumQuantity
    );
}

