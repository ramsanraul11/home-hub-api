namespace HomeHub.Application.Shopping.Queries.ListShoppingLists
{
    public sealed class ListShoppingListsHandler
    {
        private readonly IShoppingRepository _repo;
        public ListShoppingListsHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<ShoppingListDto>> Handle(Guid householdId, bool? archived, CancellationToken ct)
        {
            var lists = await _repo.ListListsAsync(householdId, archived, ct);
            return lists.Select(l => new ShoppingListDto(l.Id, l.HouseholdId, l.Name, l.IsArchived, l.CreatedAtUtc, l.CreatedByUserId)).ToList();
        }
    }
}