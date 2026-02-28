namespace HomeHub.Application.Shopping.Commands.MarkBought
{
    public sealed class MarkBoughtHandler
    {
        private readonly IShoppingRepository _repo;
        public MarkBoughtHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid householdId, Guid itemId, Guid userId, CancellationToken ct)
        {
            var item = await _repo.GetItemAsync(householdId, itemId, ct);
            if (item is null)
                return Result.Fail("shopping.item_not_found", "Item not found.");

            item.MarkBought(userId);
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }

    public sealed class UnmarkBoughtHandler
    {
        private readonly IShoppingRepository _repo;
        public UnmarkBoughtHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid householdId, Guid itemId, CancellationToken ct)
        {
            var item = await _repo.GetItemAsync(householdId, itemId, ct);
            if (item is null)
                return Result.Fail("shopping.item_not_found", "Item not found.");

            item.UnmarkBought();
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}