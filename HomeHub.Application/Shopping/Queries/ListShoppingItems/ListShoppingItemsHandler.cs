namespace HomeHub.Application.Shopping.Queries.ListShoppingItems
{
    public sealed class ListShoppingItemsHandler
    {
        private readonly IShoppingRepository _repo;
        public ListShoppingItemsHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<ShoppingListItemDto>> Handle(Guid householdId, Guid listId, bool? bought, CancellationToken ct)
        {
            var items = await _repo.ListItemsAsync(householdId, listId, bought, ct);

            return items.Select(i => new ShoppingListItemDto(
                i.Id, i.HouseholdId, i.ShoppingListId, i.Name, i.Quantity, i.Notes,
                i.IsBought, i.BoughtAtUtc, i.BoughtByUserId, i.CreatedAtUtc, i.CreatedByUserId
            )).ToList();
        }
    }
}