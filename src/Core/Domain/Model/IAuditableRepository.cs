
namespace Core.Domain.Model
{
    public interface IAuditableRepository<T> : IRepository<T> where T : IAuditableEntity<T>, IEntity<T>
    {
        void StoreWithAudit(T entity, string userName);

        void Archive(T entity, string userName);

        void DeleteWithAudit(T entity, string userName);
    }
}
