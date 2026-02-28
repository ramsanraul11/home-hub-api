namespace HomeHub.Infrastructure.Tasks.Config
{
    public sealed class TaskItemConfig : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> b)
        {
            b.ToTable("TaskItem");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.HouseholdId).IsRequired();
            b.Property(x => x.Title).HasMaxLength(200).IsRequired();
            b.Property(x => x.Description).HasMaxLength(4000);

            b.Property(x => x.Priority).IsRequired();
            b.Property(x => x.Status).IsRequired();

            b.Property(x => x.DueAtUtc);
            b.Property(x => x.CreatedByUserId).IsRequired();
            b.Property(x => x.CreatedAtUtc).IsRequired();
            b.Property(x => x.CompletedAtUtc);

            b.HasIndex(x => new { x.HouseholdId, x.Status });
            b.HasIndex(x => new { x.HouseholdId, x.DueAtUtc });
        }
    }
}

