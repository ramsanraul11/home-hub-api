namespace HomeHub.Application.Shopping.Commands.UpdateShoppingList
{
    public sealed class UpdateShoppingListHandler
    {
        private readonly IShoppingRepository _repo;
        public UpdateShoppingListHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result<ShoppingListDto>> Handle(Guid householdId, Guid listId, UpdateShoppingListCommand cmd, CancellationToken ct)
        {
            var list = await _repo.GetListAsync(householdId, listId, ct);
            if (list is null)
                return Result<ShoppingListDto>.Fail("shopping.list_not_found", "List not found.");

            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2)
                return Result<ShoppingListDto>.Fail("shopping.list_name_invalid", "List name too short.");

            list.Rename(name);
            await _repo.SaveChangesAsync(ct);

            return Result<ShoppingListDto>.Ok(new ShoppingListDto(list.Id, list.HouseholdId, list.Name, list.IsArchived, list.CreatedAtUtc, list.CreatedByUserId));
        }
    }
}