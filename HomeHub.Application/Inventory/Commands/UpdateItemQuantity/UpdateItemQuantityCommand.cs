namespace HomeHub.Application.Inventory.Commands.UpdateItemQuantity
{
    public enum QuantityOperation { Add = 1, Consume = 2 }

    public sealed record UpdateItemQuantityCommand(
        decimal Amount,
        QuantityOperation Operation
    );
}