namespace HomeHub.Application.Inventory.Projections
{
    public sealed class LowStockAlertProjector
    {
        private readonly ILowStockAlertRepository _repo;

        public LowStockAlertProjector(ILowStockAlertRepository repo) => _repo = repo;

        public async Task ProjectAsync(LowStockTriggered ev, CancellationToken ct)
        {
            // idempotencia: no crear si ya hay alerta activa
            if (await _repo.HasActiveAlertAsync(ev.HouseholdId, ev.ItemId, ct))
                return;

            var alert = LowStockAlert.Create(
                householdId: ev.HouseholdId,
                itemId: ev.ItemId,
                itemName: ev.ItemName,
                quantity: ev.Quantity,
                min: ev.MinimumQuantity,
                triggeredAtUtc: ev.OccurredAtUtc
            );

            await _repo.AddAsync(alert, ct);
            await _repo.SaveChangesAsync(ct);
        }
    }
}