namespace CrossoverLogger.IBusinessLogic
{
    using DTO;
    using System.Collections.Generic;

    public interface IRateLimitService : IService<RateLimit, long>
    {
        IServiceResult<IList<RateLimit>> GetByAppId(string appId);
        IServiceResult<long> MakeRequest(string appId);
    }
}
