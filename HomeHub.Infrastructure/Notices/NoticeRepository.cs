namespace HomeHub.Infrastructure.Notices
{
    public sealed class NoticeRepository : INoticeRepository
    {
        private readonly AppDbContext _db;
        public NoticeRepository(AppDbContext db) => _db = db;

        public Task AddAsync(Notice notice, CancellationToken ct)
        {
            _db.Notices.Add(notice);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Notice notice, CancellationToken ct)
        {
            _db.Notices.Remove(notice);
            return Task.CompletedTask;
        }

        public Task<Notice?> GetByIdAsync(Guid houseHoldId, Guid noticeId, CancellationToken ct)
            => _db.Notices.FirstOrDefaultAsync(x => x.Id == noticeId && x.HouseholdId == houseHoldId, ct);

        public async Task<IReadOnlyList<Notice>> ListAsync(
            Guid householdId,
            bool? archived,
            NoticeSeverity? severity,
            DateTime? fromUtc,
            DateTime? toUtc,
            CancellationToken ct)
        {
            var q = _db.Notices.AsNoTracking()
                .Where(n => n.HouseholdId == householdId);

            if (archived is not null)
                q = q.Where(n => n.IsArchived == archived.Value);

            if (severity is not null)
                q = q.Where(n => n.Severity == severity.Value);

            // filtro por ventana de fechas (sobre ScheduledForUtc si existe, si no CreatedAt)
            if (fromUtc is not null)
                q = q.Where(n => (n.ScheduledForUtc ?? n.CreatedAtUtc) >= fromUtc.Value);

            if (toUtc is not null)
                q = q.Where(n => (n.ScheduledForUtc ?? n.CreatedAtUtc) <= toUtc.Value);

            return await q
                .OrderBy(n => n.IsArchived) // primero activos
                .ThenBy(n => n.ScheduledForUtc == null) // primero los que tienen fecha planificada
                .ThenBy(n => n.ScheduledForUtc)
                .ThenByDescending(n => n.CreatedAtUtc)
                .ToListAsync(ct);
        }

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}