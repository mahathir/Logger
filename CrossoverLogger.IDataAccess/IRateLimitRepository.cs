namespace CrossoverLogger.IDataAccess
{
    using DTO;
    using System.Collections.Generic;

    public interface IRateLimitRepository : IRepository<RateLimit, long>
    {
        IList<RateLimit> GetByAppId(string appId);
    }
}
