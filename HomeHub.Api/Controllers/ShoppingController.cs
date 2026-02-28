namespace HomeHub.Api.Controllers
{
    [ApiController]
    [Authorize]
    public sealed class ShoppingController : ControllerBase
    {
        // Lists
        [HttpPost("households/{householdId:guid}/shopping/lists")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> CreateList(
            [FromRoute] Guid householdId,
            [FromServices] CreateShoppingListHandler handler,
            [FromBody] CreateShoppingListCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, userId, cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
        }

        [HttpGet("households/{householdId:guid}/shopping/lists")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> ListLists(
            [FromRoute] Guid householdId,
            [FromQuery] bool? archived,
            [FromServices] ListShoppingListsHandler handler,
            CancellationToken ct)
        {
            var list = await handler.Handle(householdId, archived, ct);
            return Ok(list);
        }

        [HttpPost("households/{householdId:guid}/shopping/lists/{listId:guid}/archive")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> Archive(
            [FromRoute] Guid householdId,
            [FromRoute] Guid listId,
            [FromServices] ArchiveShoppingListHandler handler,
            CancellationToken ct)
        {
            var res = await handler.Handle(householdId, listId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }

        // Items
        [HttpPost("households/{householdId:guid}/shopping/lists/{listId:guid}/items")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> AddItem(
            [FromRoute] Guid householdId,
            [FromRoute] Guid listId,
            [FromServices] AddShoppingItemHandler handler,
            [FromBody] AddShoppingItemCommand cmd,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, listId, userId, cmd, ct);
            return res.IsSuccess ? Ok(res.Value) : BadRequest(res.Error);
        }

        [HttpGet("households/{householdId:guid}/shopping/lists/{listId:guid}/items")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> ListItems(
            [FromRoute] Guid householdId,
            [FromRoute] Guid listId,
            [FromQuery] bool? bought,
            [FromServices] ListShoppingItemsHandler handler,
            CancellationToken ct)
        {
            var list = await handler.Handle(householdId, listId, bought, ct);
            return Ok(list);
        }

        [HttpPost("households/{householdId:guid}/shopping/items/{itemId:guid}/bought")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> MarkBought(
            [FromRoute] Guid householdId,
            [FromRoute] Guid itemId,
            [FromServices] MarkBoughtHandler handler,
            CancellationToken ct)
        {
            var userId = CurrentUser.GetUserId(User);
            var res = await handler.Handle(householdId, itemId, userId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }

        [HttpPost("households/{householdId:guid}/shopping/items/{itemId:guid}/unbought")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> UnmarkBought(
            [FromRoute] Guid householdId,
            [FromRoute] Guid itemId,
            [FromServices] UnmarkBoughtHandler handler,
            CancellationToken ct)
        {
            var res = await handler.Handle(householdId, itemId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }

        [HttpDelete("households/{householdId:guid}/shopping/items/{itemId:guid}")]
        [Authorize(Policy = "HouseholdMember")]
        public async Task<IActionResult> DeleteItem(
            [FromRoute] Guid householdId,
            [FromRoute] Guid itemId,
            [FromServices] DeleteShoppingItemHandler handler,
            CancellationToken ct)
        {
            var res = await handler.Handle(householdId, itemId, ct);
            return res.IsSuccess ? NoContent() : BadRequest(res.Error);
        }
    }
}