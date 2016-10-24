namespace CrossoverLogger.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using DTO;
    using IBusinessLogic;
    using IDataAccess;
    using Commons.Translation;

    public class LogService : BaseService<Log, long>, ILogService
    {
        private ILogRepository repo;
        private IApplicationService appService;

        public LogService(ILogRepository repo, IApplicationService appService) : base(repo)
        {
            this.repo = repo;
            this.appService = appService;
        }

        public IServiceResult<Log> Create(Log log)
        {
            var result = new ServiceResult<Log>();
            ValidateLog(log, result);
            if (result.ErrorMessages.Count > 0) return result;

            this.repo.Create(log);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            result.Result = log;

            return result;
        }

        public IServiceResult<bool> Update(Log entity)
        {
            var result = new ServiceResult<bool>() { Result = true };
            var existing = this.repo.Retrieve(entity.LogId);

            if (existing == null)
            {
                result.ErrorMessages.Add(nameof(Log.LogId), 
                    string.Format(MessagesResx._NotFound, EntitiesResx.Log));
                result.Result = false;
                return result;
            }

            ValidateLog(entity, result);
            if (result.ErrorMessages.Count > 0) return result;

            this.repo.Update(existing);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            return result;
        }

        #region Private Methods

        private void ValidateLog<TEntity>(Log log, ServiceResult<TEntity> result)
        {
            ValidateNullWhiteSpace(log.ApplicationId, result, nameof(Log.ApplicationId), EntitiesResx.Log);
            ValidateNullWhiteSpace(log.Level, result, nameof(Log.Level), EntitiesResx.Log);
            ValidateNullWhiteSpace(log.Logger, result, nameof(Log.Logger), EntitiesResx.Log);
            ValidateNullWhiteSpace(log.Message, result, nameof(Log.Message), EntitiesResx.Log);

            var app = this.appService.Retrieve(log.ApplicationId);
            if(app == null)
            {
                result.ErrorMessages.Add(nameof(Log.ApplicationId),
                    string.Format(MessagesResx._NotFound, EntitiesResx.Application));
            }
        }

        #endregion
    }
}
