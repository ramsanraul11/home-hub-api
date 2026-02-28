namespace HomeHub.Application.Tasks.Commands.CompleteTask
{
    public sealed class CompleteTaskHandler
    {
        private readonly ITaskRepository _repo;
        public CompleteTaskHandler(ITaskRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid taskId, CancellationToken ct)
        {
            var task = await _repo.GetByIdAsync(taskId, ct);
            if (task is null)
                return Result.Fail("task.not_found", "Task not found.");

            task.Complete();
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
