namespace CrossoverLogger.DataAccess.EF
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    public abstract class EFBaseRepository<TEntity, TKey> where TEntity : class
    {
        internal CrossoverLoggerContext Context;
        internal DbSet<TEntity> DbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="EFBaseRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public EFBaseRepository(CrossoverLoggerContext context)
        {
            this.Context = context;
            this.DbSet = this.Context.Set<TEntity>();
        }

        /// <summary>
        /// Gets the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <param name="orderBy">The order by.</param>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        /// <summary>
        /// Retrieves the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual TEntity Retrieve(TKey id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// Creates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Create(TEntity entity)
        {
            DbSet.Add(entity);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void Delete(TKey id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        /// <summary>
        /// Deletes the specified entity to delete.
        /// </summary>
        /// <param name="entityToDelete">The entity to delete.</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Updates the specified entity to update.
        /// </summary>
        /// <param name="entityToUpdate">The entity to update.</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void SaveChanges()
        {
            Context.SaveChanges();
        }

        internal IQueryable<TEntity> Sorting<TSort>(IQueryable<TEntity> query,
            Expression<Func<TEntity, TSort>> orderDefault,
            IDictionary<string, string> orderCriteria = null)
        {
            if (orderCriteria != null && orderCriteria.Count > 0)
            {
                var theType = typeof(TEntity);
                foreach (var order in orderCriteria)
                {
                    if (theType.GetProperty(order.Key) != null)
                    {
                        if ((order.Value ?? "desc").ToLower() == "desc")
                        {
                            query = query.OrderByDescending(order.Key);
                        }
                        else
                        {
                            query = query.OrderBy(order.Key);
                        }
                    }
                }
            }
            else
            {
                query = query.OrderBy(orderDefault);
            }

            return query;
        }

        internal IQueryable<TEntity> Search(IDictionary<string, string> searchCriteria, IQueryable<TEntity> query)
        {
            if (searchCriteria != null && searchCriteria.Count > 0)
            {
                var theType = typeof(TEntity);
                foreach (var criteria in searchCriteria)
                {
                    if (theType.GetProperty(criteria.Key) != null)
                    {
                        query = query.Where(SimpleComparison<TEntity>(criteria.Key, criteria.Value)).AsQueryable();
                    }
                }
            }
            return query;
        }

        internal Func<TEntity, bool> SimpleComparison<TEntity>(string property, object value)
        {
            var type = typeof(TEntity);
            var pe = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(pe, property);
            var newValue = Convert.ChangeType(value, type.GetProperty(property).PropertyType);
            var constantReference = Expression.Constant(newValue);
            return Expression.Lambda<Func<TEntity, bool>>
                (Expression.Equal(propertyReference, constantReference),
                new[] { pe }).Compile();
        }
    }
}
