namespace CrossoverLogger.IDataAccess
{
    using DTO;
    using System.Collections.Generic;

    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Create(TEntity entity);

        /// <summary>
        /// Retrieves the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        TEntity Retrieve(TKey id);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<TEntity> GetAll();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <returns></returns>
        long GetCount();

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
        void Update(TEntity entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void Delete(TKey id);

        /// <summary>
        /// Save Changes
        /// </summary>
        void SaveChanges();
    }
}
