namespace CrossoverLogger.DataAccess.EF.Repo
{
    using CrossoverLogger.DTO;
    using CrossoverLogger.IDataAccess;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Data.Entity;

    public class TokenRepository : EFBaseRepository<Token, string>, ITokenRepository
    {
        public TokenRepository(CrossoverLoggerContext context) : base(context)
        {
        }

        public void DeleteByAppId(string appId)
        {
            var deleteItems = base.DbSet.Where(a => a.ApplicationId == appId);

            if (deleteItems.Any())
            {
                foreach (var entityToDelete in deleteItems)
                {
                    if (Context.Entry(entityToDelete).State == EntityState.Detached)
                    {
                        DbSet.Attach(entityToDelete);

                    }
                }
                DbSet.RemoveRange(deleteItems);
            }
        }

        public IList<Token> GetAll()
        {
            return base.Get().ToList();
        }

        public long GetCount()
        {
            return base.DbSet.LongCount();
        }

        public PagedResult<Token> Retrieve(int pageNumber, int pageSize, 
            IDictionary<string, string> searchCriteria = null, IDictionary<string, string> orderCriteria = null)
        {
            IQueryable<Token> query = DbSet.AsQueryable();
            query = Search(searchCriteria, query);
            query = Sorting(query, a => a.AccessToken, orderCriteria);
            var count = query.LongCount();
            var result = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<Token>(result, count);
        }
    }
}
