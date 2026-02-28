namespace HomeHub.Infrastructure.Inventory
{
    public sealed class InventoryRepository : IInventoryRepository
    {
        private readonly AppDbContext _db;
        public InventoryRepository(AppDbContext db) => _db = db;

        public Task AddCategoryAsync(InventoryCategory category, CancellationToken ct)
        {
            _db.InventoryCategories.Add(category);
            return Task.CompletedTask;
        }

        public Task AddItemAsync(InventoryItem item, CancellationToken ct)
        {
            _db.InventoryItems.Add(item);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<InventoryCategory>> ListCategoriesAsync(Guid householdId, CancellationToken ct)
        {
            return await _db.InventoryCategories
                .AsNoTracking()
                .Where(c => c.HouseholdId == householdId)
                .OrderBy(c => c.Name)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<InventoryItem>> ListItemsAsync(Guid householdId, Guid? categoryId, bool? lowStockOnly, CancellationToken ct)
        {
            var q = _db.InventoryItems.AsNoTracking()
                .Where(i => i.HouseholdId == householdId);

            if (categoryId is not null)
                q = q.Where(i => i.CategoryId == categoryId);

            if (lowStockOnly == true)
                q = q.Where(i => i.Quantity <= i.MinimumQuantity);

            return await q
                .OrderBy(i => i.Name)
                .ToListAsync(ct);
        }

        public Task<InventoryItem?> GetItemAsync(Guid householdId, Guid itemId, CancellationToken ct)
            => _db.InventoryItems.FirstOrDefaultAsync(i => i.Id == itemId && i.HouseholdId == householdId, ct);

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}