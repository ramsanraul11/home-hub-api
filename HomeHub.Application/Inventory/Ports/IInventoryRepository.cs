namespace HomeHub.Application.Inventory.Ports
{
    public interface IInventoryRepository
    {
        Task AddCategoryAsync(InventoryCategory category, CancellationToken ct);
        Task AddItemAsync(InventoryItem item, CancellationToken ct);

        Task<IReadOnlyList<InventoryCategory>> ListCategoriesAsync(Guid householdId, CancellationToken ct);
        Task<IReadOnlyList<InventoryItem>> ListItemsAsync(Guid householdId, Guid? categoryId, bool? lowStockOnly, CancellationToken ct);

        Task<InventoryItem?> GetItemAsync(Guid householdId, Guid itemId, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}


