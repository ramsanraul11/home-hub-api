namespace HomeHub.Application.Tasks.Queries.ListTasks
{
    public sealed record ListTasksQuery(
        Guid HouseholdId,
        Domain.Tasks.TaskStatus? Status,
        Guid? AssignedUserId,
        DateTime? DueFromUtc,
        DateTime? DueToUtc,
        bool? Overdue
    );
}

