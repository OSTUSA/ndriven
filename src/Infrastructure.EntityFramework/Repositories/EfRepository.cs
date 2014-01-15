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
        protected readonly EntityContext _context;

        protected readonly DbSet<T> _entities;

        #region CTOR

        public EfRepository(IEntityContext context)
        {
            _context = context as EntityContext;
            _entities = _context.Set<T>();
        }

        #endregion

        #region Read Methods

        public T Get(object id)
        {
            return _entities.Find(id);
        }

        // Returns a proxy
        public T Load(object id)
        {
            // TODO : Still not sure what the EF equivelant of this is
            // In EF, to eager load you'd use .Insert()
            throw new NotImplementedException();
        }

        public EntityContext Context
        {
            get { return _context; }
        }

        // TODO : Issue here with EF? I think this will pull all records
        // In EF this should be an IQueryable<> to avoid pulling the whole table before adding filters
        public IEnumerable<T> GetAll()
        {
            return _entities; 
        }

        // TODO : include in ndriven?
        public IQueryable<T> GetPage(PageParams<T> pageParams)
        {
            // Start up the query
            var query = GetAll().AsQueryable();

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
            return _entities.Where(predicate).AsEnumerable();
        }

        public T FindOneBy(Func<T, bool> predicate)
        {
            return _entities.FirstOrDefault(predicate);
        }

        public IQueryable<T> Query()
        {
            return _entities.AsQueryable();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate);
        }

        #endregion

        #region Write Methods

        public void Store(T entity)
        {
            // If the entity is new
            if (entity.IsNew())
            {
                // Add it to the set
                _entities.Add(entity);
            }
            else
            {
                // Set the modified state
                _context.Entry(entity).State = EntityState.Modified;
            }

            // Commit the changes
            Save();
        }

        public void StoreMany(List<T> list)
        {
            _entities.AddRange(list);

            // Commit the changes
            Save();
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
            
            // Commit the changes
            Save();
        }

        public void DeleteMany(List<T> list)
        {
            _entities.RemoveRange(list);

            // Commit the changes
            Save();
        }

        protected void Save()
        {
            _context.SaveChanges();
        }

        #endregion
    }
}
