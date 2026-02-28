namespace HomeHub.Application.Inventory.Commands.CreateItem
{
    public sealed class CreateItemHandler
    {
        private readonly IInventoryRepository _repo;

        public CreateItemHandler(IInventoryRepository repo)
            => _repo = repo;

        public async Task<Result<Guid>> Handle(Guid householdId, Guid userId, CreateItemCommand cmd, CancellationToken ct)
        {
            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2)
                return Result<Guid>.Fail("inventory.item_name_invalid", "Item name too short.");

            if (cmd.Quantity < 0 || cmd.MinimumQuantity < 0)
                return Result<Guid>.Fail("inventory.quantity_invalid", "Quantity cannot be negative.");

            var item = InventoryItem.Create(
                householdId,
                cmd.CategoryId,
                name,
                cmd.Unit,
                cmd.Quantity,
                cmd.MinimumQuantity,
                userId
            );

            await _repo.AddItemAsync(item, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<Guid>.Ok(item.Id);
        }
    }
}