namespace HomeHub.Application.Shopping.Commands.ArchiveShoppingList
{
    public sealed class ArchiveShoppingListHandler
    {
        private readonly IShoppingRepository _repo;
        public ArchiveShoppingListHandler(IShoppingRepository repo) => _repo = repo;

        public async Task<Result> Handle(Guid householdId, Guid listId, CancellationToken ct)
        {
            var list = await _repo.GetListAsync(householdId, listId, ct);
            if (list is null)
                return Result.Fail("shopping.list_not_found", "List not found.");

            list.Archive();
            await _repo.SaveChangesAsync(ct);
            return Result.Ok();
        }
    }
}