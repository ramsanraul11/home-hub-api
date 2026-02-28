namespace HomeHub.Domain.Common
{
    public abstract record DomainEvent(Guid Id, DateTime OccurredAtUtc)
    {
        public static Guid NewId() => Guid.NewGuid();
    }
}