using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Core.Domain.Model;
using System.Linq.Dynamic;
using Core.Domain.Model.Pagination;


namespace Infrastructure.EntityFramework.Repositories
{
    public class EfRepository<T> : IRepository<T> where T : class, IEntity<T>
    {
        protected readonly EntityContext EntityContext;

        protected readonly DbSet<T> Entities;

        #region CTOR

        public EfRepository(IEntityContext context)
        {
            EntityContext = context as EntityContext;

            if (EntityContext != null) 
                Entities = EntityContext.Set<T>();
        }

        #endregion

        #region Read Methods

        public T Get(object id)
        {
            return Entities.Find(id);
        }

        // Returns a proxy
        public T Load(object id)
        {
            // TODO : Not sure what the EF equivelant of this is
            // If I understand this correctly (as nHibernate uses it), EF uses .Include() instead
            throw new NotImplementedException();
        }

        public EntityContext Context
        {
            get { return EntityContext; }
        }

        // Warning! This pulls the ENTIRE table
        public IEnumerable<T> GetAll()
        {
            return Entities; 
        }

        public IQueryable<T> GetPage(PageParams<T> pageParams)
        {
            // Start up the query
            var query = Query();

            // Then filter if needed
            if (pageParams.SearchPredicate != null)
                query = query.Where(pageParams.SearchPredicate);

            if (!string.IsNullOrWhiteSpace(pageParams.CustomFilter))
                query = query.Where(pageParams.CustomFilter);

            // Apply the dynamic order by
            query = query.OrderBy(pageParams.DynamicOrderBy);

            // Return the page set of records
            return query.Skip(pageParams.Count * (pageParams.Page - 1)).Take(pageParams.Count);
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            return Query(predicate).AsEnumerable();
        }

        public T FindOneBy(Func<T, bool> predicate)
        {
            return Entities.FirstOrDefault(predicate);
        }

        public IQueryable<T> Query()
        {
            return Entities.AsQueryable().AsNoTracking();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return Entities.Where(predicate);
        }

        #endregion

        #region Write Methods

        // Store without commit - for storing lists
        protected void AppendStore(T entity)
        {
            // If the entity is new
            if (entity.IsNew())
            {
                // Add it to the set
                Entities.Add(entity);
            }
            else
            {
                // Set the modified state
                EntityContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Store(T entity)
        {
            AppendStore(entity);

            // Commit the changes
            Save();
        }

        public void StoreMany(List<T> list)
        {
            foreach (var item in list)
                AppendStore(item);

            // Commit the changes
            Save();
        }

        public void Delete(T entity)
        {
            Entities.Remove(entity);
            
            // Commit the changes
            Save();
        }

        public void DeleteMany(List<T> list)
        {
            Entities.RemoveRange(list);

            // Commit the changes
            Save();
        }

        protected void Save()
        {
            EntityContext.SaveChanges();
        }

        #endregion
    }
}
