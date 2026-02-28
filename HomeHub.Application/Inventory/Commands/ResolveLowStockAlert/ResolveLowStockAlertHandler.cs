namespace HomeHub.Application.Inventory.Commands.ResolveLowStockAlert
{
    public sealed class ResolveLowStockAlertHandler
    {
        private readonly ILowStockAlertRepository _repo;

        public ResolveLowStockAlertHandler(ILowStockAlertRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid householdId, Guid alertId, Guid userId, CancellationToken ct)
        {
            var alert = await _repo.GetAsync(householdId, alertId, ct);
            if (alert is null)
                return Result.Fail("inventory.alert_not_found", "Low stock alert not found.");

            alert.Resolve(userId);
            await _repo.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}