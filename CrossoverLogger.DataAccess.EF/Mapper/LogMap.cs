namespace CrossoverLogger.DataAccess.EF.Mapper
{
    using System.Data.Entity.ModelConfiguration;
    using DTO;

    internal class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.ToTable("log");

            this.HasKey(t => t.LogId);

            this.Property(e => e.LogId)
                .HasColumnName("log_id");

            this.Property(e => e.Logger)
                .HasColumnName("logger")
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);

            this.Property(e => e.Level)
                .HasColumnName("level")
                .HasMaxLength(256)
                .IsRequired()
                .IsUnicode(false);

            this.Property(e => e.Message)
                .HasColumnName("message")
                .HasMaxLength(2048)
                .IsRequired()
                .IsUnicode(false);

            this.Property(e => e.ApplicationId)
                .HasColumnName("application_id")
                .HasMaxLength(32)
                .IsRequired()
                .IsUnicode(false);
        }
    }
}