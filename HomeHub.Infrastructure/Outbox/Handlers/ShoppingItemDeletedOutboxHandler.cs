namespace HomeHub.Infrastructure.Outbox.Handlers
{
    public sealed class ShoppingItemDeletedOutboxHandler : IOutboxEventHandler
    {
        private readonly ILogger<ShoppingItemDeletedOutboxHandler> _logger;
        public ShoppingItemDeletedOutboxHandler(ILogger<ShoppingItemDeletedOutboxHandler> logger) => _logger = logger;

        public string EventType => typeof(ShoppingItemDeleted).AssemblyQualifiedName!;

        public Task HandleAsync(string payloadJson, CancellationToken ct)
        {
            var ev = JsonSerializer.Deserialize<ShoppingItemDeleted>(payloadJson);
            if (ev is not null)
                _logger.LogInformation("Outbox: ShoppingItemDeleted household={HouseholdId} list={ListId} item={ItemId}",
                    ev.HouseholdId, ev.ListId, ev.ItemId);

            return Task.CompletedTask;
        }
    }
}

