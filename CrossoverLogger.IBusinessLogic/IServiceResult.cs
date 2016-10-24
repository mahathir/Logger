namespace CrossoverLogger.IBusinessLogic
{
    using System.Collections.Generic;

    public interface IServiceResult<TEntity>
    {
        TEntity Result { get; }
        IDictionary<string, string> ErrorMessages { get; }
        string ErrorSummary { get; }
    }
}
