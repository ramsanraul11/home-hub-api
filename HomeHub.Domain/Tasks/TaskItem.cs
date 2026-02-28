namespace HomeHub.Domain.Tasks
{
    public enum TaskPriority { Low = 1, Normal = 2, High = 3 }
    public enum TaskStatus { Open = 1, InProgress = 2, Done = 3, Cancelled = 4 }

    public sealed class TaskItem
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }

        public string Title { get; private set; } = default!;
        public string? Description { get; private set; }

        public TaskPriority Priority { get; private set; }
        public TaskStatus Status { get; private set; }

        public DateTime? DueAtUtc { get; private set; }

        public Guid CreatedByUserId { get; private set; } // claim userId (no memberId)
        public DateTime CreatedAtUtc { get; private set; }
        public DateTime? CompletedAtUtc { get; private set; }

        private TaskItem() { } // EF

        private TaskItem(Guid id, Guid householdId, string title, string? description, TaskPriority priority, DateTime? dueAtUtc, Guid createdByUserId)
        {
            Id = id;
            HouseholdId = householdId;
            Title = title;
            Description = description;
            Priority = priority;
            DueAtUtc = dueAtUtc;

            Status = TaskStatus.Open;
            CreatedByUserId = createdByUserId;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static TaskItem Create(Guid householdId, string title, string? description, TaskPriority priority, DateTime? dueAtUtc, Guid createdByUserId)
            => new(Guid.NewGuid(), householdId, title.Trim(), description, priority, dueAtUtc, createdByUserId);

        public void MarkInProgress()
        {
            if (Status == TaskStatus.Done) return;
            Status = TaskStatus.InProgress;
        }

        public void Complete()
        {
            Status = TaskStatus.Done;
            CompletedAtUtc = DateTime.UtcNow;
        }
    }
}