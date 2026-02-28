namespace HomeHub.Application.Notices.Commands.UpdateNotice
{
    public sealed class UpdateNoticeHandler
    {
        private readonly INoticeRepository _repo;
        public UpdateNoticeHandler(INoticeRepository repo) => _repo = repo;

        public async Task<Result<NoticeDto>> Handle(Guid houseHoldId, Guid noticeId, UpdateNoticeCommand cmd, CancellationToken ct)
        {
            var n = await _repo.GetByIdAsync(houseHoldId, noticeId, ct);
            if (n is null)
                return Result<NoticeDto>.Fail("notice.not_found", "Notice not found.");

            var title = (cmd.Title ?? "").Trim();
            if (title.Length < 2)
                return Result<NoticeDto>.Fail("notice.title_invalid", "Title must be at least 2 characters.");

            n.Update(title, cmd.Message, cmd.Severity, cmd.ScheduledForUtc);
            await _repo.SaveChangesAsync(ct);

            return Result<NoticeDto>.Ok(n.ToDto());
        }
    }
}