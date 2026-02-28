namespace HomeHub.Application.Notices.Commands.ArchiveNotice
{
    public sealed class ArchiveNoticeHandler
    {
        private readonly INoticeRepository _repo;
        public ArchiveNoticeHandler(INoticeRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid noticeId, CancellationToken ct)
        {
            var notice = await _repo.GetByIdAsync(noticeId, ct);
            if (notice is null)
                return Result.Fail("notice.not_found", "Notice not found.");

            notice.Archive();
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}