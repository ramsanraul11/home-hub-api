namespace HomeHub.Infrastructure.Inventory
{
    public sealed class LowStockAlertRepository : ILowStockAlertRepository
    {
        private readonly AppDbContext _db;
        public LowStockAlertRepository(AppDbContext db) => _db = db;

        public Task<bool> HasActiveAlertAsync(Guid householdId, Guid itemId, CancellationToken ct)
            => _db.LowStockAlerts.AsNoTracking()
                .AnyAsync(x => x.HouseholdId == householdId && x.ItemId == itemId && x.ResolvedAtUtc == null, ct);

        public Task AddAsync(LowStockAlert alert, CancellationToken ct)
        {
            _db.LowStockAlerts.Add(alert);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<LowStockAlert>> ListAsync(Guid householdId, bool? activeOnly, CancellationToken ct)
        {
            var q = _db.LowStockAlerts.AsNoTracking()
                .Where(x => x.HouseholdId == householdId);

            if (activeOnly == true)
                q = q.Where(x => x.ResolvedAtUtc == null);

            return await q
                .OrderByDescending(x => x.TriggeredAtUtc)
                .ToListAsync(ct);
        }

        public Task<LowStockAlert?> GetAsync(Guid householdId, Guid alertId, CancellationToken ct)
            => _db.LowStockAlerts.FirstOrDefaultAsync(x => x.Id == alertId && x.HouseholdId == householdId, ct);

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}