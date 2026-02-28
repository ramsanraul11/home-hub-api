using Microsoft.EntityFrameworkCore;

namespace HomeHub.Infrastructure.Persistence
{
    public sealed class AppDbContext
        : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<RefreshTokenEntity> RefreshTokens => Set<RefreshTokenEntity>();
        public DbSet<Household> Households => Set<Household>();
        public DbSet<HouseholdMember> HouseholdMembers => Set<HouseholdMember>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<TaskAssignment> TaskAssignments => Set<TaskAssignment>();
        public DbSet<Notice> Notices => Set<Notice>();
        public DbSet<InventoryCategory> InventoryCategories => Set<InventoryCategory>();
        public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
        public DbSet<LowStockAlert> LowStockAlerts => Set<LowStockAlert>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Ignore<DomainEvent>();

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            // Aquí luego aplicaremos Fluent Configs del ERD
            // builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var domainEntities = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entity)
                .Select(e => (Entity)e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            var events = domainEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var ev in events)
            {
                OutboxMessages.Add(new OutboxMessage
                {
                    Id = Guid.NewGuid(),
                    OccurredAtUtc = ev.OccurredAtUtc,
                    Type = ev.GetType().FullName!,
                    PayloadJson = JsonSerializer.Serialize(ev, ev.GetType())
                });
            }

            var result = await base.SaveChangesAsync(ct);

            // limpiar eventos
            foreach (var e in domainEntities)
                e.ClearDomainEvents();

            return result;
        }
    }
}
