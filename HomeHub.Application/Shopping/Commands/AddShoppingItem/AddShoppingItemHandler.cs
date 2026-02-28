namespace HomeHub.Application.Shopping.Commands.AddShoppingItem
{
    public sealed class AddShoppingItemHandler
    {
        private readonly IShoppingRepository _repo;
        public AddShoppingItemHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result<ShoppingListItemDto>> Handle(Guid householdId, Guid listId, Guid userId, AddShoppingItemCommand cmd, CancellationToken ct)
        {
            var list = await _repo.GetListAsync(householdId, listId, ct);
            if (list is null)
                return Result<ShoppingListItemDto>.Fail("shopping.list_not_found", "List not found.");

            if (list.IsArchived)
                return Result<ShoppingListItemDto>.Fail("shopping.list_archived", "List is archived.");

            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2)
                return Result<ShoppingListItemDto>.Fail("shopping.item_name_invalid", "Item name too short.");

            if (cmd.Quantity <= 0)
                return Result<ShoppingListItemDto>.Fail("shopping.quantity_invalid", "Quantity must be > 0.");

            var item = ShoppingListItem.Create(householdId, listId, name, cmd.Quantity, cmd.Notes, userId);

            await _repo.AddItemAsync(item, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<ShoppingListItemDto>.Ok(new ShoppingListItemDto(
                item.Id, item.HouseholdId, item.ShoppingListId, item.Name, item.Quantity, item.Notes,
                item.IsBought, item.BoughtAtUtc, item.BoughtByUserId, item.CreatedAtUtc, item.CreatedByUserId
            ));
        }
    }
}