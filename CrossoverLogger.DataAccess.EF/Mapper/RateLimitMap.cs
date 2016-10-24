namespace CrossoverLogger.DataAccess.EF.Mapper
{
    using System.Data.Entity.ModelConfiguration;
    using DTO;

    internal class RateLimitMap : EntityTypeConfiguration<RateLimit>
    {
        public RateLimitMap()
        {
            this.ToTable("rate_limit");

            this.HasKey(t => t.Id);

            this.Property(e => e.Id)
                .HasColumnName("id");

            this.Property(e => e.ApplicationId)
                .HasColumnName("application_id")
                .HasMaxLength(32)
                .IsUnicode(false);

            this.Property(e => e.LimitTime)
                .HasColumnName("limit_time")
                .IsRequired();

            this.Property(e => e.LimitRate)
                .HasColumnName("limit_rate")
                .IsRequired();

            this.Property(e => e.SuspendTime)
                .HasColumnName("suspend_time")
                .IsRequired();

            this.Property(e => e.RemainingRate)
                .HasColumnName("remaining_rate")
                .IsRequired();

            this.Property(e => e.StartingTime)
                .HasColumnName("starting_time")
                .HasColumnType("datetime2");

            this.Property(e => e.StartingSuspend)
                .HasColumnName("starting_suspend")
                .HasColumnType("datetime2");
        }
    }
}