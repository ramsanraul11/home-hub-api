namespace HomeHub.Application.Inventory.Commands.CreateCategory
{
    public sealed class CreateCategoryHandler
    {
        private readonly IInventoryRepository _repo;

        public CreateCategoryHandler(IInventoryRepository repo)
            => _repo = repo;

        public async Task<Result<Guid>> Handle(Guid householdId, Guid userId, CreateCategoryCommand cmd, CancellationToken ct)
        {
            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2)
                return Result<Guid>.Fail("inventory.category_name_invalid", "Category name too short.");

            var category = InventoryCategory.Create(householdId, name, userId);

            await _repo.AddCategoryAsync(category, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<Guid>.Ok(category.Id);
        }
    }
}

