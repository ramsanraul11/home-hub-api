namespace HomeHub.Application.Tasks.Commands.AssignTask
{
    public sealed record AssignTaskCommand(string Email);
    public sealed class AssignTaskHandler
    {
        private readonly ITaskRepository _tasks;
        private readonly IUserLookup _users;

        public AssignTaskHandler(ITaskRepository tasks, IUserLookup users)
            => (_tasks, _users) = (tasks, users);

        public async Task<Result> Handle(Guid taskId, AssignTaskCommand cmd, CancellationToken ct)
        {
            var task = await _tasks.GetByIdAsync(taskId, ct);
            if (task is null)
                return Result.Fail("task.not_found", "Task not found.");

            var target = await _users.FindByEmailAsync(cmd.Email, ct);
            if (!target.IsSuccess)
                return Result.Fail(target.Error!.Code, target.Error!.Message);

            // evita duplicados
            var already = await _tasks.IsAssignedToAsync(taskId, target.Value!.Id, ct);
            if (already)
                return Result.Fail("task.already_assigned", "User already assigned to this task.");

            var assignment = TaskAssignment.Create(taskId, target.Value.Id);
            await _tasks.AddAssignmentAsync(assignment, ct);

            // opcional: pasa a InProgress
            task.MarkInProgress();

            await _tasks.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}
