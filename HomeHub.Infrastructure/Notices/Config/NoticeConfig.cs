namespace HomeHub.Infrastructure.Notices.Config
{
    public sealed class NoticeConfig : IEntityTypeConfiguration<Notice>
    {
        public void Configure(EntityTypeBuilder<Notice> b)
        {
            b.ToTable("Notice");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Message).HasMaxLength(4000);

            b.Property(x => x.Severity).IsRequired();
            b.Property(x => x.ScheduledForUtc);

            b.Property(x => x.IsArchived).IsRequired();
            b.Property(x => x.ArchivedAtUtc);

            b.Property(x => x.CreatedByUserId).IsRequired();
            b.Property(x => x.CreatedAtUtc).IsRequired();

            b.HasIndex(x => new { x.HouseholdId, x.IsArchived });
            b.HasIndex(x => new { x.HouseholdId, x.Severity });
            b.HasIndex(x => new { x.HouseholdId, x.ScheduledForUtc });
            b.HasIndex(x => new { x.HouseholdId, x.CreatedAtUtc });
        }
    }
}

