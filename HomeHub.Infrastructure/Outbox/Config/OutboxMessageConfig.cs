namespace HomeHub.Infrastructure.Outbox.Config
{
    public sealed class OutboxMessageConfig : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> b)
        {
            b.ToTable("OutboxMessage");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();
            b.Property(x => x.Type).HasMaxLength(300).IsRequired();
            b.Property(x => x.PayloadJson).IsRequired();
            b.Property(x => x.OccurredAtUtc).IsRequired();
            b.HasIndex(x => x.ProcessedAtUtc);
        }
    }
}