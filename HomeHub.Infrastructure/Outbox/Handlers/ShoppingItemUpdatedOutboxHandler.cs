namespace HomeHub.Infrastructure.Outbox.Handlers
{
    public sealed class ShoppingItemUpdatedOutboxHandler : IOutboxEventHandler
    {
        private readonly ILogger<ShoppingItemUpdatedOutboxHandler> _logger;
        public ShoppingItemUpdatedOutboxHandler(ILogger<ShoppingItemUpdatedOutboxHandler> logger) => _logger = logger;

        public string EventType => typeof(ShoppingItemUpdated).AssemblyQualifiedName!;

        public Task HandleAsync(string payloadJson, CancellationToken ct)
        {
            var ev = JsonSerializer.Deserialize<ShoppingItemUpdated>(payloadJson);
            if (ev is not null)
                _logger.LogInformation("Outbox: ShoppingItemUpdated household={HouseholdId} list={ListId} item={ItemId}",
                    ev.HouseholdId, ev.ListId, ev.ItemId);

            return Task.CompletedTask;
        }
    }
}