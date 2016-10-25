namespace CrossoverLogger.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using DTO;
    using IBusinessLogic;
    using IDataAccess;
    using Commons.Translation;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Configuration;
    using Commons;

    public class ApplicationService : BaseService<Application, string>, IApplicationService
    {
        private IApplicationRepository repo;
        private IRateLimitRepository rateRepo;

        public ApplicationService(IApplicationRepository repo, IRateLimitRepository rateRepo) : base(repo)
        {
            this.repo = repo;
            this.rateRepo = rateRepo;
        }

        public IServiceResult<Application> Create(Application app)
        {
            var result = new ServiceResult<Application>();

            ValidateNullWhiteSpace(app.DisplayName, result, nameof(Application.DisplayName), 
                nameof(Application.DisplayName));
            if (result.ErrorMessages.Count > 0) return result;

            app.ApplicationId = Guid.NewGuid().ToString().Replace("-", "");
            app.Secret = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 25);

            this.repo.Create(app);

            int limitRate;
            long rateLimitTime;
            int suspendTime;

            limitRate =
                int.TryParse(ConfigurationManager.AppSettings[Constants.Config.RATE_LIMIT], out limitRate) ?
                limitRate : 60;
            rateLimitTime =
                long.TryParse(ConfigurationManager.AppSettings[Constants.Config.RATE_LIMIT_TIME], out rateLimitTime) ?
                rateLimitTime : 60;
            suspendTime =
                int.TryParse(ConfigurationManager.AppSettings[Constants.Config.SUSPEND_TIME], out suspendTime) ?
                suspendTime : 300;

            this.rateRepo.Create(
                new RateLimit {
                    Application = app,
                    LimitRate = limitRate,
                    LimitTime = rateLimitTime,
                    RemainingRate = limitRate,
                    SuspendTime = suspendTime
                });

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            result.Result = app;

            return result;
        }

        public IServiceResult<bool> Update(Application entity)
        {
            var result = new ServiceResult<bool>() { Result = true };
            var existingApp = this.repo.Retrieve(entity.ApplicationId);

            if (existingApp == null)
            {
                result.ErrorMessages.Add(nameof(Application.ApplicationId),
                    string.Format(MessagesResx._NotFound, EntitiesResx.Application));
                result.Result = false;
                return result;
            }

            ValidateNullWhiteSpace(entity.DisplayName, result, nameof(Application.DisplayName), EntitiesResx.Application);
            if (result.ErrorMessages.Count > 0) return result;

            this.repo.Update(existingApp);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            return result;
        }
    }
}
