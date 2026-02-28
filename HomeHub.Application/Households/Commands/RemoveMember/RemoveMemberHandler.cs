namespace HomeHub.Application.Households.Commands.RemoveMember
{
    public sealed class RemoveMemberHandler
    {
        private readonly IHouseholdRepository _repo;

        public RemoveMemberHandler(IHouseholdRepository repo)
            => _repo = repo;

        public async Task<Result> Handle(Guid householdId, Guid actorUserId, Guid memberId, CancellationToken ct)
        {
            var actorRole = await _repo.GetRoleForUserAsync(householdId, actorUserId, ct);
            if (actorRole is not (HouseholdRole.Owner or HouseholdRole.Admin))
                return Result.Fail("household.forbidden", "Only Owner/Admin can remove members.");

            await _repo.RemoveMemberAsync(memberId, ct);
            await _repo.SaveChangesAsync(ct);

            return Result.Ok();
        }
    }
}
