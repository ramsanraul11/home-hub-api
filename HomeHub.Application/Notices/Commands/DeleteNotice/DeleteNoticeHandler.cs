namespace HomeHub.Application.Notices.Commands.DeleteNotice
{
    public sealed class DeleteNoticeHandler
    {
        private readonly INoticeRepository _repo;
        public DeleteNoticeHandler(INoticeRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid houseHoldId, Guid noticeId, CancellationToken ct)
        {
            var n = await _repo.GetByIdAsync(houseHoldId, noticeId, ct);
            if (n is null)
                return Result.Ok(); // idempotente

            await _repo.DeleteAsync(n, ct);
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}

