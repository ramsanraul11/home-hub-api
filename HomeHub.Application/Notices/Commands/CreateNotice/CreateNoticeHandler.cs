namespace HomeHub.Application.Notices.Commands.CreateNotice
{
    public sealed class CreateNoticeHandler
    {
        private readonly INoticeRepository _repo;
        public CreateNoticeHandler(INoticeRepository repo) => _repo = repo;

        public async Task<Result<NoticeDto>> Handle(Guid householdId, Guid userId, CreateNoticeCommand cmd, CancellationToken ct)
        {
            var title = (cmd.Title ?? "").Trim();
            if (title.Length < 2)
                return Result<NoticeDto>.Fail("notice.title_invalid", "Title must be at least 2 characters.");

            var notice = Notice.Create(householdId, title, cmd.Message, cmd.Severity, cmd.ScheduledForUtc, userId);

            await _repo.AddAsync(notice, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<NoticeDto>.Ok(notice.ToDto());
        }
    }
}