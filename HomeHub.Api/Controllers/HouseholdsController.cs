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
    }
}
