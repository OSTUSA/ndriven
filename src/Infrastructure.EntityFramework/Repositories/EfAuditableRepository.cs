using Core.Domain.Model;
using Core.Domain.Model.Pagination;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;


namespace Infrastructure.EntityFramework.Repositories
{
    public class EfAuditableRepository<T> : EfRepository<T>, IAuditableRepository<T> where T : EntityBase<T>, IAuditableEntity<T>
    {
        #region CTOR

        public EfAuditableRepository(IEntityContext context) : base (context)
        {

        }

        #endregion

        #region GET METHODS

        // Override the GetPage function to filter out deleted items
        public IQueryable<T> GetPage(PageParams<T> pageParams)
        {
            // Start up the query
            var query = _entities.AsQueryable();

            if (!pageParams.ShowDeleted)
                query = query.Where(x => x.DeletedOn == default(DateTime) && x.DeletedBy == string.Empty);

            if (!pageParams.ShowArchived)
                query = query.Where(x => x.ArchivedOn == default(DateTime) && x.ArchivedBy == string.Empty);

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

        #endregion

        #region WRITE METHODS

        public void Archive(T entity, string userName)
        {
            // Name sure there is a user name
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName", "Cannot perform this action without a valid user.");

            // Store the date so it matches the updated date exactly
            var dateSnap = DateTime.Now;

            // We're toggling archive status, so see if it's already archived
            if (entity.IsArchived())
            {
                // Activate it
                entity.ArchivedBy = string.Empty;
                entity.ArchivedOn = default(DateTime);
            }
            else
            {
                // Archive it
                entity.ArchivedBy = userName;
                entity.ArchivedOn = dateSnap;
            }
            
            entity.UpdatedBy = userName;
            entity.UpdatedOn = dateSnap;

            // Set the modified state
            _context.Entry(entity).State = EntityState.Modified;

            Save();
        }

        public void DeleteWithAudit(T entity, string userName)
        {
            // Name sure there is a user name
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName", "Cannot perform this action without a valid user.");

            // Store the date so it matches the updated date exactly
            var dateSnap = DateTime.Now;

            // We're toggling delete status, so see if it's already deleted
            if (entity.IsDeleted())
            {
                // Restore it
                entity.DeletedBy = string.Empty;
                entity.DeletedOn = default(DateTime);
            }
            else
            {
                // Delete it
                entity.DeletedBy = userName;
                entity.DeletedOn = dateSnap;
            }
            
            entity.UpdatedBy = userName;
            entity.UpdatedOn = dateSnap;

            // Set the modified state
            _context.Entry(entity).State = EntityState.Modified;

            Save();
        }

        public void StoreWithAudit(T entity, string userName)
        {
            // Name sure there is a user name
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentNullException("userName", "Cannot perform this action without a valid user.");

            // If the entity is new
            if (entity.IsNew())
            {
                // Update audit properties
                entity.CreatedBy = userName;
                entity.CreatedOn = DateTime.Now;
                
                // Strip out the other audit properties in case this is a dup action
                entity.UpdatedBy = entity.ArchivedBy = entity.DeletedBy = string.Empty;
                entity.UpdatedOn = entity.ArchivedOn = entity.DeletedOn = default(DateTime);

                // Add it to the set
                _entities.Add(entity);
            }
            else
            {
                // Update audit properties
                entity.UpdatedBy = userName;
                entity.UpdatedOn = DateTime.Now;

                // Set the modified state
                _context.Entry(entity).State = EntityState.Modified;
            }

            // Commit the changes
            Save();
        }

        #endregion
    }
}
