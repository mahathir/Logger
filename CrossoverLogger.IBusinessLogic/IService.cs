namespace CrossoverLogger.IBusinessLogic
{
    using DTO;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for Service
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public interface IService<TEntity, in TKey> where TEntity : class
    {
        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>result</returns>
        IServiceResult<TEntity> Create(TEntity entity);

        /// <summary>
        /// Retrieves the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TEntity Retrieve(TKey id);

        /// <summary>
        /// Retrieves the specified page number.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="search">The search.</param>
        /// <param name="orderBy">The order by.</param>
        /// <returns></returns>
        PagedResult<TEntity> Retrieve(int pageNumber, int pageSize, IDictionary<string, string> searchCriteria = null,
            IDictionary<string, string> orderCriteria = null);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>result</returns>
        IServiceResult<bool> Update(TEntity entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        IServiceResult<bool> Delete(TKey id);
    }
}
