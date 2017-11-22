using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Trinity.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for the repository class
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Get(Expression<Func<T, bool>> predicate = null, string includeProperties = "");
        T GetById(object id);
        T Insert(T entity);
        void Delete(object id);
        void Delete(T entityToDelete);
        void Update(T entityToUpdate);
    }
}
