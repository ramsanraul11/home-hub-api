namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Authorize]
    public sealed class InventoryController : ControllerBase
    {
        // ---------- Categories ----------
        [HttpPost("households/{householdId:guid}/inventory/categories")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> CreateCategory(
            [FromRoute] Guid householdId,
            [FromServices] CreateCategoryHandler handler,
            [FromBody] CreateCategoryCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, userId, cmd, ct);
            return res.IsSuccess ? Ok(new { id = res.Value }) : BadRequest(res.Error);
        }

        [HttpGet("households/{householdId:guid}/inventory/categories")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> ListCategories(
            [FromRoute] Guid householdId,
            [FromServices] ListCategoriesHandler handler,
            CancellationToken ct)
        {
            var list = await handler.Handle(householdId, ct);
            return Ok(list);
        }

        // ---------- Items ----------
        [HttpPost("households/{householdId:guid}/inventory/items")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> CreateItem(
            [FromRoute] Guid householdId,
            [FromServices] CreateItemHandler handler,
            [FromBody] CreateItemCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, userId, cmd, ct);
            return res.IsSuccess ? Ok(new { id = res.Value }) : BadRequest(res.Error);
        }

        [HttpGet("households/{householdId:guid}/inventory/items")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> ListItems(
            [FromRoute] Guid householdId,
            [FromQuery] Guid? categoryId,
            [FromQuery] bool? lowStockOnly,
            [FromServices] ListItemsHandler handler,
            CancellationToken ct)
        {
            var list = await handler.Handle(householdId, categoryId, lowStockOnly, ct);
            return Ok(list);
        }

        // ---------- Quantity (add / consume) ----------
        public sealed record QuantityRequest(decimal Amount);

        [HttpPost("households/{householdId:guid}/inventory/items/{itemId:guid}/add")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> AddQuantity(
            [FromRoute] Guid householdId,
            [FromRoute] Guid itemId,
            [FromServices] UpdateItemQuantityHandler handler,
            [FromBody] QuantityRequest req,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);

            var cmd = new UpdateItemQuantityCommand(req.Amount, QuantityOperation.Add);
            var res = await handler.Handle(householdId, itemId, userId, cmd, ct);

            return res.IsSuccess ? Ok(new { lowStockTriggered = res.Value }) : BadRequest(res.Error);
        }

        [HttpPost("households/{householdId:guid}/inventory/items/{itemId:guid}/consume")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> ConsumeQuantity(
            [FromRoute] Guid householdId,
            [FromRoute] Guid itemId,
            [FromServices] UpdateItemQuantityHandler handler,
            [FromBody] QuantityRequest req,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);

            var cmd = new UpdateItemQuantityCommand(req.Amount, QuantityOperation.Consume);
            var res = await handler.Handle(householdId, itemId, userId, cmd, ct);

            return res.IsSuccess ? Ok(new { lowStockTriggered = res.Value }) : BadRequest(res.Error);
        }
    }
}
