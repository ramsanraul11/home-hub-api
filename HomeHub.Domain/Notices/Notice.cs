namespace HomeHub.Domain.Notices
{
    public enum NoticeSeverity { Info = 1, Warning = 2, Urgent = 3 }

    public sealed class Notice
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }

        public string Title { get; private set; } = default!;
        public string? Message { get; private set; }
        public NoticeSeverity Severity { get; private set; }

        public DateTime? ScheduledForUtc { get; private set; } // “fontanero viene mañana”
        public bool IsArchived { get; private set; }
        public DateTime? ArchivedAtUtc { get; private set; }

        public Guid CreatedByUserId { get; private set; }
        public DateTime CreatedAtUtc { get; private set; }

        private Notice() { } // EF

        private Notice(Guid id, Guid householdId, string title, string? message, NoticeSeverity severity, DateTime? scheduledForUtc, Guid createdByUserId)
        {
            Id = id;
            HouseholdId = householdId;
            Title = title;
            Message = message;
            Severity = severity;
            ScheduledForUtc = scheduledForUtc;

            IsArchived = false;
            CreatedByUserId = createdByUserId;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static Notice Create(Guid householdId, string title, string? message, NoticeSeverity severity, DateTime? scheduledForUtc, Guid createdByUserId)
            => new(Guid.NewGuid(), householdId, title.Trim(), message, severity, scheduledForUtc, createdByUserId);
        public void Archive()
        {
            if (IsArchived) return;
            IsArchived = true;
            ArchivedAtUtc = DateTime.UtcNow;
        }
        public void Update(string title, string? message, NoticeSeverity severity, DateTime? scheduledForUtc)
        {
            Title = title.Trim();
            Message = message;
            Severity = severity;
            ScheduledForUtc = scheduledForUtc;
        }
    }
}
