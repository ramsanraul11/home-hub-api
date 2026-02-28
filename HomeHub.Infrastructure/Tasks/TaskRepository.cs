namespace HomeHub.Infrastructure.Tasks
{
    public sealed class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _db;
        public TaskRepository(AppDbContext db) => _db = db;

        public Task AddAsync(TaskItem task, CancellationToken ct)
        {
            _db.Tasks.Add(task);
            return Task.CompletedTask;
        }

        public Task<TaskItem?> GetByIdAsync(Guid taskId, CancellationToken ct)
            => _db.Tasks.FirstOrDefaultAsync(x => x.Id == taskId, ct);

        public async Task<IReadOnlyList<TaskItem>> ListAsync(Guid householdId, Domain.Tasks.TaskStatus? status, Guid? assignedUserId, CancellationToken ct)
        {
            var q = _db.Tasks.AsNoTracking().Where(t => t.HouseholdId == householdId);

            if (status is not null)
                q = q.Where(t => t.Status == status.Value);

            if (assignedUserId is not null)
            {
                q = q.Join(
                    _db.TaskAssignments.AsNoTracking().Where(a => a.UserId == assignedUserId.Value),
                    t => t.Id,
                    a => a.TaskItemId,
                    (t, _) => t
                );
            }

            return await q.OrderByDescending(t => t.CreatedAtUtc).ToListAsync(ct);
        }

        public Task<bool> IsAssignedToAsync(Guid taskId, Guid userId, CancellationToken ct)
            => _db.TaskAssignments.AsNoTracking()
                .AnyAsync(a => a.TaskItemId == taskId && a.UserId == userId, ct);

        public Task AddAssignmentAsync(TaskAssignment assignment, CancellationToken ct)
        {
            _db.TaskAssignments.Add(assignment);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
