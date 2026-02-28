namespace HomeHub.Application.Notices.Queries.ListNotices
{
    public sealed class ListNoticesHandler
    {
        private readonly INoticeRepository _repo;
        public ListNoticesHandler(INoticeRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<NoticeDto>> Handle(
            Guid householdId,
            bool? archived,
            NoticeSeverity? severity,
            DateTime? fromUtc,
            DateTime? toUtc,
            CancellationToken ct)
        {
            var list = await _repo.ListAsync(householdId, archived, severity, fromUtc, toUtc, ct);
            return list.Select(x => x.ToDto()).ToList();
        }
    }
}
