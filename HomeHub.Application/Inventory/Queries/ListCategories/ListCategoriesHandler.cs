namespace HomeHub.Application.Inventory.Queries.ListCategories
{
    public sealed record InventoryCategoryDto(Guid Id, string Name);

    public sealed class ListCategoriesHandler
    {
        private readonly IInventoryRepository _repo;

        public ListCategoriesHandler(IInventoryRepository repo)
            => _repo = repo;

        public async Task<IReadOnlyList<InventoryCategoryDto>> Handle(Guid householdId, CancellationToken ct)
        {
            var categories = await _repo.ListCategoriesAsync(householdId, ct);

            return categories
                .Select(c => new InventoryCategoryDto(c.Id, c.Name))
                .ToList();
        }
    }
}