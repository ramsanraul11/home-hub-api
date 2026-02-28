namespace HomeHub.Api.Controllers
{
    //TODO: Eliminar tareas
    [ApiController]
    [Authorize]
    public sealed class TasksController : ControllerBase
    {
        // Crear tarea (miembro del hogar)
        [HttpPost("households/{householdId:guid}/tasks")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Create(
            [FromRoute] Guid householdId,
            [FromServices] CreateTaskHandler handler,
            [FromBody] CreateTaskCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, userId, cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
        }

        // Listar tareas (miembro del hogar) + filtros ?status=Open&assignedUserId=...
        [HttpGet("households/{householdId:guid}/tasks")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> List(
        [FromRoute] Guid householdId,
        [FromQuery] Domain.Tasks.TaskStatus? status,
        [FromQuery] Guid? assignedUserId,
        [FromQuery] DateTime? dueFromUtc,
        [FromQuery] DateTime? dueToUtc,
        [FromQuery] bool? overdue,
        [FromServices] ListTasksHandler handler,
        CancellationToken ct)
        {
            var q = new ListTasksQuery(
                HouseholdId: householdId,
                Status: status,
                AssignedUserId: assignedUserId,
                DueFromUtc: dueFromUtc,
                DueToUtc: dueToUtc,
                Overdue: overdue
            );

            var list = await handler.Handle(q, ct);
            return Ok(list);
        }

        // Asignar tarea (Owner/Admin) - por ahora lo restringimos
        [HttpPost("households/{householdId:guid}/tasks/{taskId:guid}/assign")]
        [Authorize(Policy = "HouseholdAdminOrOwner")]
        public async Task<IActionResult> Assign(
            [FromRoute] Guid householdId, // for policy route read
            [FromRoute] Guid taskId,
            [FromServices] AssignTaskHandler handler,
            [FromBody] AssignTaskCommand cmd,
            CancellationToken ct)
        {
            var res = await handler.Handle(taskId, cmd, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }

        // Completar tarea (miembro del hogar)
        [HttpPost("households/{householdId:guid}/tasks/{taskId:guid}/complete")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Complete(
            [FromRoute] Guid householdId, // for policy route read
            [FromRoute] Guid taskId,
            [FromServices] CompleteTaskHandler handler,
            CancellationToken ct)
        {
            var res = await handler.Handle(taskId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }
    }
}