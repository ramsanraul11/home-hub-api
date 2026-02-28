namespace HomeHub.Application.Notices.Ports
{
    public interface INoticeRepository
    {
        Task AddAsync(Notice notice, CancellationToken ct);
        Task<Notice?> GetByIdAsync(Guid noticeId, CancellationToken ct);

        Task<IReadOnlyList<Notice>> ListAsync(
            Guid householdId,
            bool? archived,
            NoticeSeverity? severity,
            DateTime? fromUtc,
            DateTime? toUtc,
            CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}
