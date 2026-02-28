namespace HomeHub.Application.Tasks.Dtos
{
    public sealed record TaskDto(
        Guid Id,
        Guid HouseholdId,
        string Title,
        string? Description,
        TaskPriority Priority,
        Domain.Tasks.TaskStatus Status,
        DateTime? DueAtUtc,
        DateTime CreatedAtUtc,
        DateTime? CompletedAtUtc
    );

    public static class TaskMapping
    {
        public static TaskDto ToDto(this TaskItem t) =>
            new(t.Id, t.HouseholdId, t.Title, t.Description, t.Priority, t.Status, t.DueAtUtc, t.CreatedAtUtc, t.CompletedAtUtc);
    }
}
