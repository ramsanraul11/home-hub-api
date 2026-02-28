namespace HomeHub.Application.Tasks.Commands.CreateTask
{
    public sealed record CreateTaskCommand(
        string Title,
        string? Description,
        TaskPriority Priority,
        DateTime? DueAtUtc
        );
}
