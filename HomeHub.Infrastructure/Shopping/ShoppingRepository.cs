namespace HomeHub.Infrastructure.Shopping
{
    public sealed class ShoppingRepository : IShoppingRepository
    {
        private readonly AppDbContext _db;
        public ShoppingRepository(AppDbContext db) => _db = db;

        public Task AddListAsync(ShoppingList list, CancellationToken ct)
        {
            _db.ShoppingLists.Add(list);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<ShoppingList>> ListListsAsync(Guid householdId, bool? archived, CancellationToken ct)
        {
            var q = _db.ShoppingLists.AsNoTracking()
                .Where(x => x.HouseholdId == householdId);

            if (archived is not null)
                q = q.Where(x => x.IsArchived == archived.Value);

            return await q.OrderByDescending(x => x.CreatedAtUtc).ToListAsync(ct);
        }

        public Task<ShoppingList?> GetListAsync(Guid householdId, Guid listId, CancellationToken ct)
            => _db.ShoppingLists.FirstOrDefaultAsync(x => x.Id == listId && x.HouseholdId == householdId, ct);

        public Task AddItemAsync(ShoppingListItem item, CancellationToken ct)
        {
            _db.ShoppingListItems.Add(item);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<ShoppingListItem>> ListItemsAsync(Guid householdId, Guid listId, bool? bought, CancellationToken ct)
        {
            var q = _db.ShoppingListItems.AsNoTracking()
                .Where(x => x.HouseholdId == householdId && x.ShoppingListId == listId);

            if (bought is not null)
                q = q.Where(x => x.IsBought == bought.Value);

            return await q
                .OrderBy(x => x.IsBought)
                .ThenByDescending(x => x.CreatedAtUtc)
                .ToListAsync(ct);
        }

        public Task<ShoppingListItem?> GetItemAsync(Guid householdId, Guid itemId, CancellationToken ct)
            => _db.ShoppingListItems.FirstOrDefaultAsync(x => x.Id == itemId && x.HouseholdId == householdId, ct);

        public Task DeleteItemAsync(ShoppingListItem item, CancellationToken ct)
        {
            _db.ShoppingListItems.Remove(item);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
