namespace CrossoverLogger.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using DTO;
    using IBusinessLogic;
    using IDataAccess;
    using Commons.Translation;
    using System.Configuration;
    using Commons;

    public class RateLimitService : BaseService<RateLimit, long>, IRateLimitService
    {
        private IRateLimitRepository repo;
        private IApplicationService appService;

        public RateLimitService(IRateLimitRepository repo, IApplicationService appService) : base(repo)
        {
            this.repo = repo;
            this.appService = appService;
        }

        public IServiceResult<RateLimit> Create(RateLimit rateLimit)
        {
            var result = new ServiceResult<RateLimit>();
            ValidateNullWhiteSpace(rateLimit.ApplicationId, result, nameof(RateLimit.ApplicationId),
                EntitiesResx.RateLimit);
            if (result.ErrorMessages.Count > 0) return result;

            this.repo.Create(rateLimit);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            result.Result = rateLimit;

            return result;
        }

        public IServiceResult<IList<RateLimit>> GetByAppId(string appId)
        {
            return new ServiceResult<IList<RateLimit>>()
            {
                Result = this.repo.GetByAppId(appId)
            };
        }

        public IServiceResult<long> MakeRequest(string appId)
        {
            var result = new ServiceResult<long>();
            var rates = this.repo.GetByAppId(appId);
            var currentCallTime = DateTime.UtcNow;

            if (rates == null || rates.Count == 0)
            {
                var app = this.appService.Retrieve(appId);

                int limitRate;
                long rateLimitTime;
                int suspendTime;

                limitRate = 
                    int.TryParse(ConfigurationManager.AppSettings[Constants.Config.RateLimit], out limitRate) ?
                    limitRate : 60;
                rateLimitTime = 
                    long.TryParse(ConfigurationManager.AppSettings[Constants.Config.RateLimitTime], out rateLimitTime) ?
                    rateLimitTime : 60;
                suspendTime =
                    int.TryParse(ConfigurationManager.AppSettings[Constants.Config.SuspendTime], out suspendTime) ?
                    suspendTime : 300;

                this.repo.Create(
                new RateLimit
                {
                    Application = app,
                    LimitRate = limitRate,
                    LimitTime = rateLimitTime,
                    RemainingRate = limitRate - 1,
                    SuspendTime = suspendTime,
                    StartingTime = currentCallTime
                });
            }
            else
            {
                for (var i = 0; i < rates.Count; i++)
                {
                    var rate = rates[i];

                    if (!rate.StartingTime.HasValue)
                    {
                        rate.StartingTime = currentCallTime;
                        rate.RemainingRate = rate.LimitRate - 1;
                        Update(rate);
                    }
                    else
                    {
                        var isSuspended = rate.StartingSuspend.HasValue ?
                            rate.StartingSuspend.Value.AddSeconds(rate.SuspendTime) > currentCallTime :
                            false;
                        var isExceedLimitTime = rate.StartingTime.Value.AddSeconds(rate.LimitTime) < currentCallTime;
                        var isExceedLimitRate = rate.RemainingRate == 0;

                        if (isSuspended)
                        {
                            result.Result = rate.SuspendTime;
                            rate.StartingSuspend = currentCallTime;
                            Update(rate);
                        }
                        else if (isExceedLimitTime)
                        {
                            rate.RemainingRate = rate.LimitRate - 1;
                            var diff = currentCallTime - rate.StartingTime.Value;
                            rate.StartingTime = rate.StartingTime.Value
                                .AddSeconds((long)diff.TotalSeconds / rate.LimitTime * rate.LimitTime);
                            Update(rate);
                        }
                        else if (!isExceedLimitRate)
                        {
                            rate.RemainingRate = rate.RemainingRate - 1;
                            Update(rate);
                        }
                        else
                        {
                            result.Result = rate.SuspendTime;
                            rate.StartingSuspend = currentCallTime;
                            rate.RemainingRate = rate.LimitRate;
                            Update(rate);
                        }
                    }
                }
            }

            return result;
        }

        public IServiceResult<bool> Update(RateLimit entity)
        {
            var result = new ServiceResult<bool>() { Result = true };
            var existing = this.repo.Retrieve(entity.Id);

            if (existing == null)
            {
                result.ErrorMessages.Add(nameof(RateLimit.Id), 
                    string.Format(MessagesResx._NotFound, EntitiesResx.RateLimit));
                result.Result = false;
                return result;
            }

            ValidateNullWhiteSpace(entity.ApplicationId, result, nameof(RateLimit.ApplicationId),
                EntitiesResx.RateLimit);
            if (result.ErrorMessages.Count > 0) return result;

            this.repo.Update(existing);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            return result;
        }
    }
}
