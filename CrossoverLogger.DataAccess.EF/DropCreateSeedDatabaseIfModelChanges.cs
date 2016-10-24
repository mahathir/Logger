namespace CrossoverLogger.DataAccess.EF
{
    using System.Data.Entity;

    public class DropCreateSeedDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<CrossoverLoggerContext>
    {
        protected override void Seed(CrossoverLoggerContext context)
        {
            // TODO: initialize data here.
        }
    }
}
