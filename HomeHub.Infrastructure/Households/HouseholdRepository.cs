namespace HomeHub.Infrastructure.Households
{
    public sealed class HouseholdRepository : IHouseholdRepository
    {
        private readonly AppDbContext _db;
        public HouseholdRepository(AppDbContext db) => _db = db;

        public Task AddAsync(Household household, CancellationToken ct)
        {
            _db.Households.Add(household);
            return Task.CompletedTask;
        }

        public Task AddMemberAsync(HouseholdMember member, CancellationToken ct)
        {
            _db.HouseholdMembers.Add(member);
            return Task.CompletedTask;
        }

        public async Task<IReadOnlyList<Household>> GetForUserAsync(Guid userId, CancellationToken ct)
        {
            // join membership -> households
            return await _db.HouseholdMembers
                .AsNoTracking()
                .Where(m => m.UserId == userId && m.Status == MemberStatus.Active)
                .Join(_db.Households.AsNoTracking(),
                    m => m.HouseholdId,
                    h => h.Id,
                    (_, h) => h)
                .OrderBy(h => h.CreatedAtUtc)
                .ToListAsync(ct);
        }

        public Task<Household?> GetByIdAsync(Guid householdId, CancellationToken ct)
            => _db.Households.AsNoTracking().FirstOrDefaultAsync(h => h.Id == householdId, ct);

        public Task<bool> IsMemberAsync(Guid householdId, Guid userId, CancellationToken ct)
            => _db.HouseholdMembers.AsNoTracking()
                .AnyAsync(m => m.HouseholdId == householdId && m.UserId == userId && m.Status == MemberStatus.Active, ct);

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);

        public async Task<HouseholdRole?> GetRoleForUserAsync(Guid householdId, Guid userId, CancellationToken ct)
        {
            var m = await _db.HouseholdMembers.AsNoTracking()
                .FirstOrDefaultAsync(x => x.HouseholdId == householdId && x.UserId == userId && x.Status == MemberStatus.Active, ct);

            return m is null ? null : m.Role;
        }

        public Task<bool> MemberExistsAsync(Guid householdId, Guid userId, CancellationToken ct)
            => _db.HouseholdMembers.AsNoTracking()
                .AnyAsync(x => x.HouseholdId == householdId && x.UserId == userId && x.Status == MemberStatus.Active, ct);

        public async Task<IReadOnlyList<(Guid MemberId, Guid UserId, HouseholdRole Role)>>
            GetActiveMembersAsync(Guid householdId, CancellationToken ct)
        {
            return await _db.HouseholdMembers
                .AsNoTracking()
                .Where(x => x.HouseholdId == householdId && x.Status == MemberStatus.Active)
                .Select(x => new ValueTuple<Guid, Guid, HouseholdRole>(x.Id, x.UserId, x.Role))
                .ToListAsync(ct);
        }

        public async Task RemoveMemberAsync(Guid memberId, CancellationToken ct)
        {
            var member = await _db.HouseholdMembers.FirstOrDefaultAsync(x => x.Id == memberId, ct);
            if (member is null) return;

            member.Status = MemberStatus.Removed;
            member.LeftAtUtc = DateTime.UtcNow;
        }
    }
}
