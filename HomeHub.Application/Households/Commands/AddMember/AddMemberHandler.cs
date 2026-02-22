namespace HomeHub.Application.Households.Commands.AddMember
{
    public sealed class AddMemberHandler
    {
        private readonly IHouseholdRepository _repo;
        private readonly IUserLookup _users;

        public AddMemberHandler(IHouseholdRepository repo, IUserLookup users)
            => (_repo, _users) = (repo, users);

        public async Task<Result<MemberDto>> Handle(Guid householdId, Guid actorUserId, AddMemberCommand cmd, CancellationToken ct)
        {
            // 1) Actor role check (Owner/Admin)
            var actorRole = await _repo.GetRoleForUserAsync(householdId, actorUserId, ct);
            if (actorRole is null)
                return Result<MemberDto>.Fail("household.forbidden", "Not a member of this household.");

            if (actorRole is not (HouseholdRole.Owner or HouseholdRole.Admin))
                return Result<MemberDto>.Fail("household.forbidden", "Only Owner/Admin can add members.");

            // 2) Resolve target user
            var target = await _users.FindByEmailAsync(cmd.Email, ct);
            if (!target.IsSuccess)
                return Result<MemberDto>.Fail(target.Error!.Code, target.Error!.Message);

            // 3) Prevent duplicates
            var exists = await _repo.MemberExistsAsync(householdId, target.Value!.Id, ct);
            if (exists)
                return Result<MemberDto>.Fail("household.member_exists", "User is already a member.");

            // 4) Create membership
            var role = cmd.Role;
            if (role == HouseholdRole.Owner && actorRole != HouseholdRole.Owner)
                return Result<MemberDto>.Fail("household.forbidden", "Only an Owner can assign Owner role.");

            var member = HouseholdMember.CreateMember(householdId, target.Value.Id, role);
            await _repo.AddMemberAsync(member, ct);
            await _repo.SaveChangesAsync(ct);

            return Result<MemberDto>.Ok(new MemberDto(member.Id, target.Value.Id, target.Value.Email, role));
        }
    }
}
