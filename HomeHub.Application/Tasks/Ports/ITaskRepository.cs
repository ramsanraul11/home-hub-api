namespace HomeHub.Application.Tasks.Ports
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem task, CancellationToken ct);
        Task<TaskItem?> GetByIdAsync(Guid taskId, CancellationToken ct);

        Task<IReadOnlyList<TaskItem>> ListAsync(ListTasksQuery query, CancellationToken ct);

        Task<bool> IsAssignedToAsync(Guid taskId, Guid userId, CancellationToken ct);

        Task AddAssignmentAsync(TaskAssignment assignment, CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}
