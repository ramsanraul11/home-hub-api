namespace HomeHub.Application.Shopping.Ports
{
    public interface IShoppingRepository
    {
        // Lists
        Task AddListAsync(ShoppingList list, CancellationToken ct);
        Task<IReadOnlyList<ShoppingList>> ListListsAsync(Guid householdId, bool? archived, CancellationToken ct);
        Task<ShoppingList?> GetListAsync(Guid householdId, Guid listId, CancellationToken ct);

        // Items
        Task AddItemAsync(ShoppingListItem item, CancellationToken ct);
        Task<IReadOnlyList<ShoppingListItem>> ListItemsAsync(Guid householdId, Guid listId, bool? bought, CancellationToken ct);
        Task<ShoppingListItem?> GetItemAsync(Guid householdId, Guid itemId, CancellationToken ct);
        Task DeleteItemAsync(ShoppingListItem item, CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}