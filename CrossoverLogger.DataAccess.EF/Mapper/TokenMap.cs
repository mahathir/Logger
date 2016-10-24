namespace CrossoverLogger.DataAccess.EF.Mapper
{
    using System.Data.Entity.ModelConfiguration;
    using DTO;

    internal class TokenMap : EntityTypeConfiguration<Token>
    {
        public TokenMap()
        {
            this.ToTable("token");

            this.HasKey(t => t.AccessToken);

            this.Property(e => e.AccessToken)
                .HasColumnName("token")
                .HasMaxLength(32)
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