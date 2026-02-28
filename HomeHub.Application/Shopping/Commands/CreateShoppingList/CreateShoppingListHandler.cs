namespace HomeHub.Application.Shopping.Commands.CreateShoppingList
{
    public sealed class CreateShoppingListHandler
    {
        private readonly IShoppingRepository _repo;
        public CreateShoppingListHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result<ShoppingListDto>> Handle(Guid householdId, Guid userId, CreateShoppingListCommand cmd, CancellationToken ct)
        {
            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2)
                return Result<ShoppingListDto>.Fail("shopping.list_name_invalid", "List name too short.");

            var list = ShoppingList.Create(householdId, name, userId);

            await _repo.AddListAsync(list, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<ShoppingListDto>.Ok(new ShoppingListDto(list.Id, list.HouseholdId, list.Name, list.IsArchived, list.CreatedAtUtc, list.CreatedByUserId));
        }
    }
}