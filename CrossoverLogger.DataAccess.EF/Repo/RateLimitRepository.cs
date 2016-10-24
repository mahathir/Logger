namespace CrossoverLogger.DataAccess.EF.Repo
{
    using CrossoverLogger.DTO;
    using CrossoverLogger.IDataAccess;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class RateLimitRepository : EFBaseRepository<RateLimit, long>, IRateLimitRepository
    {
        public RateLimitRepository(CrossoverLoggerContext context) : base(context)
        {
        }

        public IList<RateLimit> GetAll()
        {
            return base.Get().ToList();
        }

        public IList<RateLimit> GetByAppId(string appId)
        {
            return base.DbSet.Where(a => a.ApplicationId == appId).ToList();
        }

        public long GetCount()
        {
            return base.DbSet.LongCount();
        }

        public PagedResult<RateLimit> Retrieve(int pageNumber, int pageSize, 
            IDictionary<string, string> searchCriteria = null, IDictionary<string, string> orderCriteria = null)
        {
            IQueryable<RateLimit> query = DbSet.AsQueryable();
            query = Search(searchCriteria, query);
            query = Sorting(query, a => a.Id, orderCriteria);
            var count = query.LongCount();
            var result = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<RateLimit>(result, count);
        }
    }
}
