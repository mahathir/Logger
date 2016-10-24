namespace CrossoverLogger.DataAccess.EF.Repo
{
    using DTO;
    using IDataAccess;
    using System.Collections.Generic;
    using System.Linq;
    using System;

    public class ApplicationRepository : EFBaseRepository<Application, string>, IApplicationRepository
    {
        public ApplicationRepository(CrossoverLoggerContext context) : base(context)
        {
        }

        public IList<Application> GetAll()
        {
            return base.Get().ToList();
        }

        public long GetCount()
        {
            return base.DbSet.LongCount();
        }

        public PagedResult<Application> Retrieve(int pageNumber, int pageSize, 
            IDictionary<string, string> searchCriteria = null, IDictionary<string, string> orderCriteria = null)
        {
            IQueryable<Application> query = DbSet.AsQueryable();
            query = Search(searchCriteria, query);
            query = Sorting(query, a => a.ApplicationId, orderCriteria);
            var count = query.LongCount();
            var result = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<Application>(result, count);
        }
    }
}
