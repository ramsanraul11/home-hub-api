namespace HomeHub.Domain.Tasks
{
    public sealed class TaskAssignment
    {
        public Guid Id { get; private set; }
        public Guid TaskItemId { get; private set; }
        public Guid UserId { get; private set; } // asignación a usuario (Identity)
        public DateTime AssignedAtUtc { get; private set; }

        private TaskAssignment() { } // EF

        private TaskAssignment(Guid id, Guid taskItemId, Guid userId)
        {
            Id = id;
            TaskItemId = taskItemId;
            UserId = userId;
            AssignedAtUtc = DateTime.UtcNow;
        }

        public static TaskAssignment Create(Guid taskItemId, Guid userId)
            => new(Guid.NewGuid(), taskItemId, userId);
    }
}