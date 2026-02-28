namespace HomeHub.Application.Inventory.Queries.ListItems
{
    public sealed record InventoryItemDto(
        Guid Id,
        string Name,
        Guid? CategoryId,
        InventoryUnit Unit,
        decimal Quantity,
        decimal MinimumQuantity,
        bool IsLowStock
    );

    public sealed class ListItemsHandler
    {
        private readonly IInventoryRepository _repo;

        public ListItemsHandler(IInventoryRepository repo)
            => _repo = repo;

        public async Task<IReadOnlyList<InventoryItemDto>> Handle(
            Guid householdId,
            Guid? categoryId,
            bool? lowStockOnly,
            CancellationToken ct)
        {
            var items = await _repo.ListItemsAsync(householdId, categoryId, lowStockOnly, ct);

            return items
                .Select(i => new InventoryItemDto(
                    i.Id,
                    i.Name,
                    i.CategoryId,
                    i.Unit,
                    i.Quantity,
                    i.MinimumQuantity,
                    i.IsLowStock()
                ))
                .ToList();
        }
    }
}