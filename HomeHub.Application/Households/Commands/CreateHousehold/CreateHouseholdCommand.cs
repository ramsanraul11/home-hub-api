namespace HomeHub.Application.Households.Commands.CreateHousehold
{
    public sealed record CreateHouseholdCommand(string Name, string? CurrencyCode);
    public sealed record HouseholdDto(Guid Id, string Name, string CurrencyCode);
}
