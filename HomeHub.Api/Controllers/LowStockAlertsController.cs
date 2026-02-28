namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Authorize]
    public sealed class LowStockAlertsController : ControllerBase
    {
        [HttpGet("households/{householdId:guid}/inventory/low-stock-alerts")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> List(
            [FromRoute] Guid householdId,
            [FromQuery] bool? activeOnly,
            [FromServices] ListLowStockAlertsHandler handler,
            CancellationToken ct)
        {
            var list = await handler.Handle(householdId, activeOnly, ct);
            return Ok(list);
        }

        [HttpPost("households/{householdId:guid}/inventory/low-stock-alerts/{alertId:guid}/resolve")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Resolve(
            [FromRoute] Guid householdId,
            [FromRoute] Guid alertId,
            [FromServices] ResolveLowStockAlertHandler handler,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, alertId, userId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }
    }
}