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

        public async Task<IReadOnlyList<TaskItem>> ListAsync(ListTasksQuery q, CancellationToken ct)
        {
            var query = _db.Tasks.AsNoTracking()
                .Where(t => t.HouseholdId == q.HouseholdId);

            if (q.Status is not null)
                query = query.Where(t => t.Status == q.Status.Value);

            // Due range
            if (q.DueFromUtc is not null)
                query = query.Where(t => t.DueAtUtc != null && t.DueAtUtc >= q.DueFromUtc.Value);

            if (q.DueToUtc is not null)
                query = query.Where(t => t.DueAtUtc != null && t.DueAtUtc <= q.DueToUtc.Value);

            // Overdue
            if (q.Overdue == true)
            {
                var now = DateTime.UtcNow;
                query = query.Where(t =>
                    t.DueAtUtc != null &&
                    t.DueAtUtc < now &&
                    t.Status != Domain.Tasks.TaskStatus.Done &&
                    t.Status != Domain.Tasks.TaskStatus.Cancelled
                );
            }

            // Assigned user
            if (q.AssignedUserId is not null)
            {
                var assigned = _db.TaskAssignments.AsNoTracking()
                    .Where(a => a.UserId == q.AssignedUserId.Value);

                query = query.Join(assigned, t => t.Id, a => a.TaskItemId, (t, _) => t);
            }

            return await query
                .OrderBy(t => t.DueAtUtc == null)     // primero las que tienen due date
                .ThenBy(t => t.DueAtUtc)
                .ThenByDescending(t => t.CreatedAtUtc)
                .ToListAsync(ct);
        }

        public Task<bool> IsAssignedToAsync(Guid taskId, Guid userId, CancellationToken ct)
            => _db.TaskAssignments.AsNoTracking().AnyAsync(a => a.TaskItemId == taskId && a.UserId == userId, ct);

        public Task AddAssignmentAsync(TaskAssignment assignment, CancellationToken ct)
        {
            _db.TaskAssignments.Add(assignment);
            return Task.CompletedTask;
        }

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
