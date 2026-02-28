namespace HomeHub.Application.Households.Queries.ListMember
{
    public sealed record MemberListItem(Guid MemberId, Guid UserId, HouseholdRole Role);

    public sealed class ListMembersHandler
    {
        private readonly IHouseholdRepository _repo;

        public ListMembersHandler(IHouseholdRepository repo)
            => _repo = repo;

        public async Task<IReadOnlyList<MemberListItem>> Handle(Guid householdId, CancellationToken ct)
        {
            var members = await _repo.GetActiveMembersAsync(householdId, ct);

            return members
                .Select(x => new MemberListItem(x.MemberId, x.UserId, x.Role))
                .ToList();
        }
    }
}
