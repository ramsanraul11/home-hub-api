namespace HomeHub.Application.Tasks.Commands.CreateTask
{
    public sealed class CreateTaskHandler
    {
        private readonly ITaskRepository _repo;

        public CreateTaskHandler(ITaskRepository repo) => _repo = repo;

        public async Task<Result<TaskDto>> Handle(Guid householdId, Guid userId, CreateTaskCommand cmd, CancellationToken ct)
        {
            var title = (cmd.Title ?? "").Trim();
            if (title.Length < 2)
                return Result<TaskDto>.Fail("task.title_invalid", "Title must be at least 2 characters.");

            var task = TaskItem.Create(
                householdId,
                title,
                description: cmd.Description,
                priority: cmd.Priority,
                dueAtUtc: cmd.DueAtUtc,
                createdByUserId: userId
            );

            await _repo.AddAsync(task, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<TaskDto>.Ok(task.ToDto());
        }
    }
}
