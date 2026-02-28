namespace HomeHub.Infrastructure.Outbox
{
    public interface IOutboxEventHandler
    {
        /// <summary>Assembly-qualified type name que maneja (o FullName si quieres fallback)</summary>
        string EventType { get; }

        Task HandleAsync(string payloadJson, CancellationToken ct);
    }
}
