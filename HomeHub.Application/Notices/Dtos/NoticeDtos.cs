namespace HomeHub.Application.Notices.Dtos
{
    public sealed record NoticeDto(
        Guid Id,
        Guid HouseholdId,
        string Title,
        string? Message,
        NoticeSeverity Severity,
        DateTime? ScheduledForUtc,
        bool IsArchived,
        DateTime CreatedAtUtc,
        Guid CreatedByUserId,
        DateTime? ArchivedAtUtc
    );

    public static class NoticeMapping
    {
        public static NoticeDto ToDto(this Notice n) =>
            new(n.Id, n.HouseholdId, n.Title, n.Message, n.Severity, n.ScheduledForUtc, n.IsArchived,
                n.CreatedAtUtc, n.CreatedByUserId, n.ArchivedAtUtc);
    }
}
