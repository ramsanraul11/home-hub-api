namespace HomeHub.Application.Shopping.Commands.UpdateShoppingItem
{
    public sealed class UpdateShoppingItemHandler
    {
        private readonly IShoppingRepository _repo;
        public UpdateShoppingItemHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result<ShoppingListItemDto>> Handle(Guid householdId, Guid itemId, Guid userId, UpdateShoppingItemCommand cmd, CancellationToken ct)
        {
            var item = await _repo.GetItemAsync(householdId, itemId, ct);
            if (item is null)
                return Result<ShoppingListItemDto>.Fail("shopping.item_not_found", "Item not found.");

            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2)
                return Result<ShoppingListItemDto>.Fail("shopping.item_name_invalid", "Item name too short.");
            if (cmd.Quantity <= 0)
                return Result<ShoppingListItemDto>.Fail("shopping.quantity_invalid", "Quantity must be > 0.");

            item.Update(name, cmd.Quantity, cmd.Notes, userId);

            await _repo.SaveChangesAsync(ct);

            return Result<ShoppingListItemDto>.Ok(new ShoppingListItemDto(
                item.Id, item.HouseholdId, item.ShoppingListId, item.Name, item.Quantity, item.Notes,
                item.IsBought, item.BoughtAtUtc, item.BoughtByUserId, item.CreatedAtUtc, item.CreatedByUserId
            ));
        }
    }
}


