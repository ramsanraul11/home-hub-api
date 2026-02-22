using HomeHub.Application.Households.Ports;

namespace HomeHub.Application.Households.Commands.CreateHousehold
{
    public sealed class CreateHouseholdHandler
    {
        private readonly IHouseholdRepository _repo;

        public CreateHouseholdHandler(IHouseholdRepository repo) => _repo = repo;

        public async Task<Result<HouseholdDto>> Handle(CreateHouseholdCommand cmd, Guid userId, CancellationToken ct)
        {
            var name = (cmd.Name ?? "").Trim();
            if (name.Length < 2) return Result<HouseholdDto>.Fail("household.name_invalid", "Name must be at least 2 characters.");

            var currency = string.IsNullOrWhiteSpace(cmd.CurrencyCode) ? "EUR" : cmd.CurrencyCode.Trim().ToUpperInvariant();
            if (currency.Length != 3) return Result<HouseholdDto>.Fail("household.currency_invalid", "Currency code must be 3 letters.");

            var household = Household.Create(name, userId, currency);
            var owner = HouseholdMember.CreateOwner(household.Id, userId);

            await _repo.AddAsync(household, ct);
            await _repo.AddMemberAsync(owner, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<HouseholdDto>.Ok(new HouseholdDto(household.Id, household.Name, household.CurrencyCode));
        }
    }
}
