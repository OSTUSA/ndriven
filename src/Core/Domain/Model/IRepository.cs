using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Domain.Model
{
    public interface IRepository<T> where T : IEntity<T>
    {
        T Get(object id);

        T Load(object id);

        IEnumerable<T> GetAll();

        IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate);

        T FindOneBy(Func<T, bool> predicate);

        IQueryable<T> Query();

        IQueryable<T> Query(Expression<Func<T, bool>> predicate); 

        void Store(T entity);

        void Delete(T entity);
    }
}
