namespace CrossoverLogger.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using DTO;
    using IBusinessLogic;
    using IDataAccess;
    using Commons.Translation;

    public class TokenService : BaseService<Token, string>, ITokenService
    {
        private ITokenRepository repo;
        private IApplicationService appService;

        public TokenService(ITokenRepository repo, IApplicationService appService) : base(repo)
        {
            this.repo = repo;
            this.appService = appService;
        }

        public IServiceResult<Token> Create(Token rateLimit)
        {
            var result = new ServiceResult<Token>();
            ValidateNullWhiteSpace(rateLimit.ApplicationId, result, nameof(Token.ApplicationId),
                EntitiesResx.Token);
            if (result.ErrorMessages.Count > 0) return result;

            rateLimit.AccessToken = Guid.NewGuid().ToString().Replace("-", "");
            this.repo.Create(rateLimit);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            result.Result = rateLimit;
            return result;
        }

        public IServiceResult<Token> GenerateNewToken(string applicationId, string secret)
        {
            var result = new ServiceResult<Token>();
            var existingApp = appService.Retrieve(applicationId);

            if (existingApp == null)
            {
                result.ErrorMessages.Add(nameof(Application.ApplicationId),
                    string.Format(MessagesResx._NotFound, EntitiesResx.Application)); ;
                return result;
            }

            if (existingApp.Secret != secret)
            {
                result.ErrorMessages.Add(nameof(Application.Secret),
                    string.Format(MessagesResx._NotFound, EntitiesResx.Application));
                return result;
            }

            var newToken = new Token
            {
                AccessToken = Guid.NewGuid().ToString().Replace("-",""),
                ApplicationId = applicationId
            };
            this.repo.Create(newToken);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            result.Result = newToken;

            return result;
        }

        public IServiceResult<bool> Update(Token entity)
        {
            var result = new ServiceResult<bool>() { Result = true };
            var existing = this.repo.Retrieve(entity.AccessToken);

            if (existing == null)
            {
                result.ErrorMessages.Add(nameof(Token.AccessToken), 
                    string.Format(MessagesResx._NotFound, EntitiesResx.Token));
                result.Result = false;
                return result;
            }

            ValidateNullWhiteSpace(entity.ApplicationId, result, nameof(Token.ApplicationId),
                EntitiesResx.Token);
            if (result.ErrorMessages.Count > 0) return result;

            this.repo.Update(existing);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            return result;
        }

        public IServiceResult<bool> DeleteByAppId(string appId)
        {
            var result = new ServiceResult<bool>() { Result = true };
            this.repo.DeleteByAppId(appId);

            SaveChanges(result);
            if (result.ErrorMessages.Count > 0) return result;

            return result;
        }
    }
}
