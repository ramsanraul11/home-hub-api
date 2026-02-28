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
            // simple loop
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessBatch(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Outbox processing failed");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ProcessBatch(CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var projector = scope.ServiceProvider.GetRequiredService<LowStockAlertProjector>();

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
                    if (msg.Type == typeof(LowStockTriggered).FullName)
                    {
                        var ev = JsonSerializer.Deserialize<LowStockTriggered>(msg.PayloadJson);
                        if (ev is not null)
                            await projector.ProjectAsync(ev, ct);
                    }

                    msg.ProcessedAtUtc = DateTime.UtcNow;
                    msg.Error = null;
                }
                catch (Exception ex)
                {
                    msg.Error = ex.Message;
                    // No marcamos ProcessedAtUtc para reintentar, o si prefieres evitar bucles,
                    // puedes marcarlo procesado con error y reintentar manualmente.
                    _logger.LogError(ex, "Error processing outbox message {Id}", msg.Id);
                }
            }

            await db.SaveChangesAsync(ct);
        }
    }
}