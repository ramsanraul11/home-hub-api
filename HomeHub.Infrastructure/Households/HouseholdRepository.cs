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
    }
}
