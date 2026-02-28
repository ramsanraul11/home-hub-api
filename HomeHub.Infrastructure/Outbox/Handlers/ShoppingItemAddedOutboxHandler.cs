namespace HomeHub.Infrastructure.Outbox.Handlers
{
    public sealed class ShoppingItemAddedOutboxHandler : IOutboxEventHandler
    {
        private readonly ILogger<ShoppingItemAddedOutboxHandler> _logger;
        public ShoppingItemAddedOutboxHandler(ILogger<ShoppingItemAddedOutboxHandler> logger) => _logger = logger;

        public string EventType => typeof(ShoppingItemAdded).AssemblyQualifiedName!;

        public Task HandleAsync(string payloadJson, CancellationToken ct)
        {
            var ev = JsonSerializer.Deserialize<ShoppingItemAdded>(payloadJson);
            if (ev is not null)
                _logger.LogInformation("Outbox: ShoppingItemAdded household={HouseholdId} list={ListId} item={ItemId} name={Name}",
                    ev.HouseholdId, ev.ListId, ev.ItemId, ev.Name);

            return Task.CompletedTask;
        }
    }
}
