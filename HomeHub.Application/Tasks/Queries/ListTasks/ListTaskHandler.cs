namespace HomeHub.Application.Tasks.Queries.ListTasks
{
    public sealed class ListTasksHandler
    {
        private readonly ITaskRepository _repo;
        public ListTasksHandler(ITaskRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<TaskDto>> Handle(Guid householdId, Domain.Tasks.TaskStatus? status, Guid? assignedUserId, CancellationToken ct)
        {
            var tasks = await _repo.ListAsync(householdId, status, assignedUserId, ct);
            return tasks.Select(t => t.ToDto()).ToList();
        }
    }
}
