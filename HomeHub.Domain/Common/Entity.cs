namespace HomeHub.Domain.Common
{
    public abstract class Entity
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents;

        protected void Raise(DomainEvent ev) => _domainEvents.Add(ev);
        public void ClearDomainEvents() => _domainEvents.Clear();
    }
}