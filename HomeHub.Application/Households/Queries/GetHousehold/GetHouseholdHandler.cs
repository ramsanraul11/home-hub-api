namespace HomeHub.Application.Households.Queries.GetHousehold
{
    public sealed class GetHouseholdHandler
    {
        private readonly IHouseholdRepository _repo;
        public GetHouseholdHandler(IHouseholdRepository repo) => _repo = repo;

        public async Task<HouseholdDto?> Handle(Guid householdId, CancellationToken ct)
        {
            var h = await _repo.GetByIdAsync(householdId, ct);
            return h is null ? null : new HouseholdDto(h.Id, h.Name, h.CurrencyCode);
        }
    }
}
