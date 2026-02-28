namespace HomeHub.Infrastructure.Outbox.Handlers
{
    public sealed class ShoppingItemBoughtStateChangedOutboxHandler : IOutboxEventHandler
    {
        private readonly ILogger<ShoppingItemBoughtStateChangedOutboxHandler> _logger;
        public ShoppingItemBoughtStateChangedOutboxHandler(ILogger<ShoppingItemBoughtStateChangedOutboxHandler> logger) => _logger = logger;

        public string EventType => typeof(ShoppingItemBoughtStateChanged).AssemblyQualifiedName!;

        public Task HandleAsync(string payloadJson, CancellationToken ct)
        {
            var ev = JsonSerializer.Deserialize<ShoppingItemBoughtStateChanged>(payloadJson);
            if (ev is not null)
                _logger.LogInformation("Outbox: ShoppingItemBoughtStateChanged household={HouseholdId} item={ItemId} bought={IsBought}",
                    ev.HouseholdId, ev.ItemId, ev.IsBought);

            return Task.CompletedTask;
        }
    }
}