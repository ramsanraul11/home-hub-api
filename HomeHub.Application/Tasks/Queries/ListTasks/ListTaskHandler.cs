namespace HomeHub.Application.Tasks.Queries.ListTasks
{
    public sealed class ListTasksHandler
    {
        private readonly ITaskRepository _repo;
        public ListTasksHandler(ITaskRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<TaskDto>> Handle(ListTasksQuery q, CancellationToken ct)
        {
            var tasks = await _repo.ListAsync(q, ct);
            return tasks.Select(t => t.ToDto()).ToList();
        }
    }
}
