namespace HomeHub.Domain.Household
{
    public sealed class Household
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public string CurrencyCode { get; private set; } = "EUR";
        public DateTime CreatedAtUtc { get; private set; }
        public Guid CreatedByUserId { get; private set; }

        private Household() { } // EF
        private Household(Guid id, string name, Guid createdByUserId, string currencyCode)
        {
            Id = id;
            Name = name;
            CreatedByUserId = createdByUserId;
            CurrencyCode = currencyCode;
            CreatedAtUtc = DateTime.UtcNow;
        }

        public static Household Create(string name, Guid createdByUserId, string currencyCode = "EUR")
            => new(Guid.NewGuid(), name.Trim(), createdByUserId, currencyCode);
    }
}
