namespace HomeHub.Application.Inventory.Queries.ListLowStockAlerts
{
    public sealed class ListLowStockAlertsHandler
    {
        private readonly ILowStockAlertRepository _repo;

        public ListLowStockAlertsHandler(ILowStockAlertRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<LowStockAlertDto>> Handle(Guid householdId, bool? activeOnly, CancellationToken ct)
        {
            var alerts = await _repo.ListAsync(householdId, activeOnly, ct);

            return alerts.Select(a => new LowStockAlertDto(
                a.Id,
                a.ItemId,
                a.ItemName,
                a.Quantity,
                a.MinimumQuantity,
                a.TriggeredAtUtc,
                a.ResolvedAtUtc,
                a.ResolvedByUserId
            )).ToList();
        }
    }
}