namespace CrossoverLogger.DataAccess.EF
{
    using System.Data.Entity;
    using DTO;
    using Mapper;

    public partial class CrossoverLoggerContext : DbContext
    {
        static CrossoverLoggerContext()
        {
            Database.SetInitializer<CrossoverLoggerContext>(new CreateDatabaseIfNotExists<CrossoverLoggerContext>());
        }

        public CrossoverLoggerContext()
            : base("name=CrossoverLoggerContext")
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<Log> Logs { get; set; }
        public virtual DbSet<RateLimit> RateLimits { get; set; }
        public virtual DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ApplicationMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new RateLimitMap());
            modelBuilder.Configurations.Add(new TokenMap());
        }
    }
}
