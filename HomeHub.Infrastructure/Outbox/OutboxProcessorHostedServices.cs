namespace HomeHub.Infrastructure.Outbox
{
    public sealed class OutboxProcessorHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxProcessorHostedService> _logger;

        public OutboxProcessorHostedService(IServiceScopeFactory scopeFactory, ILogger<OutboxProcessorHostedService> logger)
            => (_scopeFactory, _logger) = (scopeFactory, logger);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try { await ProcessBatch(stoppingToken); }
                catch (Exception ex) { _logger.LogError(ex, "Outbox processing failed"); }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessBatch(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var handlers = scope.ServiceProvider.GetServices<IOutboxEventHandler>().ToList();

            var batch = await db.OutboxMessages
                .Where(x => x.ProcessedAtUtc == null)
                .OrderBy(x => x.OccurredAtUtc)
                .Take(20)
                .ToListAsync(ct);

            if (batch.Count == 0) return;

            foreach (var msg in batch)
            {
                try
                {
                    var handler = FindHandler(handlers, msg.Type);

                    if (handler is null)
                    {
                        // Estrategia: marcar como procesado para que no bloquee (queda trazabilidad)
                        msg.ProcessedAtUtc = DateTime.UtcNow;
                        msg.Error = $"No handler for type: {msg.Type}";
                        continue;
                    }

                    await handler.HandleAsync(msg.PayloadJson, ct);

                    msg.ProcessedAtUtc = DateTime.UtcNow;
                    msg.Error = null;
                }
                catch (Exception ex)
                {
                    msg.Error = ex.Message;
                    _logger.LogError(ex, "Error processing outbox message {Id} type={Type}", msg.Id, msg.Type);

                    // Estrategia: NO marcar ProcessedAtUtc para reintentar.
                    // Si prefieres evitar loops, marca procesado con error.
                }
            }

            await db.SaveChangesAsync(ct);
        }

        private static IOutboxEventHandler? FindHandler(List<IOutboxEventHandler> handlers, string typeName)
        {
            // match exact (AssemblyQualifiedName)
            var h = handlers.FirstOrDefault(x => x.EventType == typeName);
            if (h is not null) return h;

            // fallback por si en DB quedó FullName
            return handlers.FirstOrDefault(x =>
                x.EventType.Contains(typeName, StringComparison.Ordinal));
        }
    }
}


