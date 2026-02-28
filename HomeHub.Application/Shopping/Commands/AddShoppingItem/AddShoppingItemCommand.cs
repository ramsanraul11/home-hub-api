namespace HomeHub.Application.Shopping.Commands.AddShoppingItem
{
    public sealed record AddShoppingItemCommand(
        string Name,
        decimal Quantity,
        string? Notes
    );
}