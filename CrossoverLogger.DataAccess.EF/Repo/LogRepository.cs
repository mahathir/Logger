namespace CrossoverLogger.DataAccess.EF.Repo
{
    using DTO;
    using IDataAccess;
    using System.Collections.Generic;
    using System.Linq;

    public class LogRepository : EFBaseRepository<Log, long>, ILogRepository
    {
        public LogRepository(CrossoverLoggerContext context) : base(context)
        {
        }

        public IList<Log> GetAll()
        {
            return base.Get().ToList();
        }

        public long GetCount()
        {
            return base.DbSet.LongCount();
        }

        public PagedResult<Log> Retrieve(int pageNumber, int pageSize,
            IDictionary<string, string> searchCriteria = null, IDictionary<string, string> orderCriteria = null)
        {
            IQueryable<Log> query = DbSet.AsQueryable();
            query = Search(searchCriteria, query);
            query = Sorting(query, a => a.LogId, orderCriteria);
            var count = query.LongCount();
            var result = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<Log>(result, count);
        }
    }
}
