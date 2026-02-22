namespace HomeHub.Domain.Household
{
    public enum HouseholdRole { Owner = 1, Admin = 2, Member = 3 }
    public enum MemberStatus { Pending = 1, Active = 2, Removed = 3 }

    public sealed class HouseholdMember
    {
        public Guid Id { get; private set; }
        public Guid HouseholdId { get; private set; }
        public Guid UserId { get; private set; }
        public HouseholdRole Role { get; private set; }
        public MemberStatus Status { get; private set; }
        public DateTime JoinedAtUtc { get; private set; }
        public DateTime? LeftAtUtc { get; private set; }

        private HouseholdMember() { } // EF

        private HouseholdMember(Guid id, Guid householdId, Guid userId, HouseholdRole role)
        {
            Id = id;
            HouseholdId = householdId;
            UserId = userId;
            Role = role;
            Status = MemberStatus.Active;
            JoinedAtUtc = DateTime.UtcNow;
        }

        public static HouseholdMember CreateOwner(Guid householdId, Guid userId)
            => new(Guid.NewGuid(), householdId, userId, HouseholdRole.Owner);

        public static HouseholdMember CreateMember(Guid householdId, Guid userId, HouseholdRole role)
            => new(Guid.NewGuid(), householdId, userId, role);
    }
}
