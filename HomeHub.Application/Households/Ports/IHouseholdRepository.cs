namespace HomeHub.Application.Households.Ports
{
    public interface IHouseholdRepository
    {
        Task AddAsync(Household household, CancellationToken ct);
        Task AddMemberAsync(HouseholdMember member, CancellationToken ct);

        Task<IReadOnlyList<Household>> GetForUserAsync(Guid userId, CancellationToken ct);
        Task<Household?> GetByIdAsync(Guid householdId, CancellationToken ct);

        Task<bool> IsMemberAsync(Guid householdId, Guid userId, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
        Task<HouseholdRole?> GetRoleForUserAsync(Guid householdId, Guid userId, CancellationToken ct);
        Task<bool> MemberExistsAsync(Guid householdId, Guid userId, CancellationToken ct);
    }
}
