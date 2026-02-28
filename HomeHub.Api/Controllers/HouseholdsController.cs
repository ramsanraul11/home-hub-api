namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Route("households")]
    [Authorize]
    public sealed class HouseholdsController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromServices] CreateHouseholdHandler handler,
            [FromBody] CreateHouseholdCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(cmd, userId, ct);
            return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
        }

        [HttpGet]
        public async Task<IActionResult> ListMine(
            [FromServices] ListMyHouseholdsHandler handler,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var list = await handler.Handle(userId, ct);
            return Ok(list);
        }

        [HttpGet("{householdId:guid}")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Get([FromRoute] Guid householdId, [FromServices] GetHouseholdHandler handler, CancellationToken ct)
        {
            var dto = await handler.Handle(householdId, ct);
            return dto is null ? NotFound() : Ok(dto);
        }

        [HttpPost("{householdId:guid}/members")]
        [Authorize(Policy = "HouseholdAdminOrOwner")]
        public async Task<IActionResult> AddMember(
            [FromRoute] Guid householdId,
            [FromServices] AddMemberHandler handler,
            [FromBody] AddMemberCommand cmd,
            CancellationToken ct)
        {
            var actorUserId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, actorUserId, cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
        }
        [HttpGet("{householdId:guid}/members")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> ListMembers([FromRoute] Guid householdId, [FromServices] ListMembersHandler handler, CancellationToken ct)
        {
            var list = await handler.Handle(householdId, ct);
            return Ok(list);
        }

        [HttpDelete("{householdId:guid}/members/{memberId:guid}")]
        [Authorize(Policy = "HouseholdAdminOrOwner")]
        public async Task<IActionResult> RemoveMember(
            [FromRoute] Guid householdId,
            [FromRoute] Guid memberId,
            [FromServices] RemoveMemberHandler handler,
            CancellationToken ct)
        {
            var actorUserId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, actorUserId, memberId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }
    }
}
