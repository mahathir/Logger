namespace CrossoverLogger.IBusinessLogic
{
    using DTO;

    public interface ITokenService : IService<Token, string>
    {
        IServiceResult<Token> GenerateNewToken(string applicationId, string secret);
        IServiceResult<bool> DeleteByAppId(string appId);
    }
}
