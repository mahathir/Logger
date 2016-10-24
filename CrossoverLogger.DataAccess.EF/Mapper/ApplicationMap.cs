namespace CrossoverLogger.DataAccess.EF.Mapper
{
    using System.Data.Entity.ModelConfiguration;
    using DTO;

    internal class ApplicationMap : EntityTypeConfiguration<Application>
    {
        public ApplicationMap()
        {
            this.ToTable("application");
            this.HasKey(t => t.ApplicationId);

            this.Property(e => e.ApplicationId)
                .HasColumnName("application_id")
                .HasMaxLength(32)
                .IsUnicode(false);

            this.Property(e => e.DisplayName)
                .HasColumnName("display_name")
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);

            this.Property(e => e.Secret)
                .HasColumnName("secret")
                .IsRequired()
                .HasMaxLength(25)
                .IsUnicode(false);

            this.HasMany(e => e.Logs)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete();

            this.HasMany(e => e.RateLimit)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete();

            this.HasMany(e => e.Tokens)
                .WithRequired(e => e.Application)
                .WillCascadeOnDelete();
        }
    }
}