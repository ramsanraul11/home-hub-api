namespace HomeHub.Application.Households.Queries.ListMyHouseholds
{
    public sealed class ListMyHouseholdsHandler
    {
        private readonly IHouseholdRepository _repo;
        public ListMyHouseholdsHandler(IHouseholdRepository repo) => _repo = repo;

        public async Task<IReadOnlyList<HouseholdDto>> Handle(Guid userId, CancellationToken ct)
        {
            var households = await _repo.GetForUserAsync(userId, ct);
            return households.Select(h => new HouseholdDto(h.Id, h.Name, h.CurrencyCode)).ToList();
        }
    }
}
