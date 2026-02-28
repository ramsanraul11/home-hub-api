namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Authorize]
    public sealed class NoticesController : ControllerBase
    {
        [HttpPost("households/{householdId:guid}/notices")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Create(
            [FromRoute] Guid householdId,
            [FromServices] CreateNoticeHandler handler,
            [FromBody] CreateNoticeCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, userId, cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
        }

        [HttpGet("households/{householdId:guid}/notices")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> List(
            [FromRoute] Guid householdId,
            [FromQuery] bool? archived,
            [FromQuery] NoticeSeverity? severity,
            [FromQuery] DateTime? fromUtc,
            [FromQuery] DateTime? toUtc,
            [FromServices] ListNoticesHandler handler,
            CancellationToken ct)
        {
            var list = await handler.Handle(householdId, archived, severity, fromUtc, toUtc, ct);
            return Ok(list);
        }

        [HttpPost("households/{householdId:guid}/notices/{noticeId:guid}/archive")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Archive(
            [FromRoute] Guid householdId, // para que la policy lea route
            [FromRoute] Guid noticeId,
            [FromServices] ArchiveNoticeHandler handler,
            CancellationToken ct)
        {
            var res = await handler.Handle(noticeId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }
    }
}
