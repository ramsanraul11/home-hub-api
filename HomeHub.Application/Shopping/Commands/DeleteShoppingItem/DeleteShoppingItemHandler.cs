namespace HomeHub.Application.Shopping.Commands.DeleteShoppingItem
{
    public sealed class DeleteShoppingItemHandler
    {
        private readonly IShoppingRepository _repo;
        public DeleteShoppingItemHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid householdId, Guid itemId, CancellationToken ct)
        {
            var item = await _repo.GetItemAsync(householdId, itemId, ct);
            if (item is null)
                return Result.Ok(); // idempotente

            await _repo.DeleteItemAsync(item, ct);
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}