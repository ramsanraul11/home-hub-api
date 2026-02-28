namespace HomeHub.Application.Notices.Commands.UpdateNotice
{
    public sealed record UpdateNoticeCommand(
        string Title,
        string? Message,
        NoticeSeverity Severity,
        DateTime? ScheduledForUtc
    );
}