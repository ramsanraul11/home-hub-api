namespace HomeHub.Application.Shopping.Commands.UpdateShoppingItem
{
    public sealed record UpdateShoppingItemCommand(
        string Name,
        decimal Quantity,
        string? Notes
    );
}
