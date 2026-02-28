namespace HomeHub.Application.Inventory.Ports
{
    public interface ILowStockAlertRepository
    {
        Task<bool> HasActiveAlertAsync(Guid householdId, Guid itemId, CancellationToken ct);
        Task AddAsync(LowStockAlert alert, CancellationToken ct);
        Task<IReadOnlyList<LowStockAlert>> ListAsync(Guid householdId, bool? activeOnly, CancellationToken ct);
        Task<LowStockAlert?> GetAsync(Guid householdId, Guid alertId, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}

