namespace HomeHub.Application.Inventory.Ports
{
    public interface ILowStockAlertRepository
    {
        Task<bool> HasActiveAlertAsync(Guid householdId, Guid itemId, CancellationToken ct);
        Task AddAsync(LowStockAlert alert, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}

