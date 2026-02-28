namespace HomeHub.Infrastructure.Tasks.Config
{
    public sealed class TaskAssignmentConfig : IEntityTypeConfiguration<TaskAssignment>
    {
        public void Configure(EntityTypeBuilder<TaskAssignment> b)
        {
            b.ToTable("TaskAssignment");
            b.HasKey(x => x.Id);
            b.Property(x => x.Id).ValueGeneratedNever();

            b.Property(x => x.TaskItemId).IsRequired();
            b.Property(x => x.UserId).IsRequired();
            b.Property(x => x.AssignedAtUtc).IsRequired();

            b.HasIndex(x => new { x.TaskItemId, x.UserId }).IsUnique();
            b.HasIndex(x => x.UserId);

            b.HasOne<TaskItem>()
                .WithMany()
                .HasForeignKey(x => x.TaskItemId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne<AppUser>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}