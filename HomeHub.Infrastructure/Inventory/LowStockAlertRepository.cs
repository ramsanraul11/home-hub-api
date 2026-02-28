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

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}