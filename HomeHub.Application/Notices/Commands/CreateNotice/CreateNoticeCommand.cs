namespace HomeHub.Application.Notices.Commands.CreateNotice
{
    public sealed record CreateNoticeCommand
    (
        string Title,
        string? Message,
        NoticeSeverity Severity,
        DateTime? ScheduledForUtc
    );
}
