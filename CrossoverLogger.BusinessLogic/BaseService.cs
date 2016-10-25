namespace CrossoverLogger.BusinessLogic
{
    using Commons.Translation;
    using DTO;
    using IBusinessLogic;
    using IDataAccess;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Linq;

    public abstract class BaseService<TEntity, TKey> where TEntity : class
    {
        private IRepository<TEntity, TKey> repo;
        public BaseService(IRepository<TEntity, TKey> repo)
        {
            this.repo = repo;
        }

        internal static void ValidateNullWhiteSpace<T>(string value, IServiceResult<T> result,
            string prop, string entity)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                result.ErrorMessages.Add(prop, string.Format(MessagesResx._CantBe_, entity,
                    CommonsResx.Empty));
            }
        }

        public IServiceResult<bool> Delete(TKey id)
        {
            var result = new ServiceResult<bool>();
            var existing = repo.Retrieve(id);
            if (existing != null)
            {
                repo.Delete(existing);
                repo.SaveChanges();
                result.Result = true;
            }
            return result;
        }

        public TEntity Retrieve(TKey id)
        {
            return repo.Retrieve(id);
        }

        public PagedResult<TEntity> Retrieve(int pageNumber, int pageSize,
            IDictionary<string, string> searchCriteria = null, IDictionary<string, string> orderCriteria = null)
        {
            return repo.Retrieve(pageNumber, pageSize, searchCriteria, orderCriteria);
        }

        protected void SaveChanges<T>(IServiceResult<T> result)
        {
            try
            {
                repo.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var messages = ex.EntityValidationErrors.SelectMany(a => a.ValidationErrors)
                    .GroupBy(a => a.PropertyName)
                    .ToDictionary(a => a.Key, b => b.Select(c => c.ErrorMessage).ToList());

                foreach (var msg in messages)
                {
                    result.ErrorMessages.Add(msg.Key, string.Join(Environment.NewLine, msg.Value));
                }

                result.ErrorMessages.Add("Error", ex.Message);
            }
        }
    }
}
