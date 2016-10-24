namespace CrossoverLogger.IDataAccess
{
    using DTO;

    public interface ITokenRepository : IRepository<Token, string>
    {
        void DeleteByAppId(string appId);
    }
}
