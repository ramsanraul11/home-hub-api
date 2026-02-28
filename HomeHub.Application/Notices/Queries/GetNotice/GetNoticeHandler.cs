namespace HomeHub.Application.Notices.Queries.GetNotice
{
    public sealed class GetNoticeHandler
    {
        private readonly INoticeRepository _repo;
        public GetNoticeHandler(INoticeRepository repo) => _repo = repo;

        public async Task<NoticeDto?> Handle(Guid houseHoldId, Guid noticeId, CancellationToken ct)
        {
            var n = await _repo.GetByIdAsync(houseHoldId, noticeId, ct);
            return n is null ? null : n.ToDto();
        }
    }
}
