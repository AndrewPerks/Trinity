using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Trinity.DataAccess.Interfaces;

namespace Trinity.DataAccess.Concrete
{
    /// <summary>
    /// Repository mediates between the data source layer and the business layers of the application
    /// </summary>
    /// <typeparam name="TEntity">Type of repository</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly TrinityContext _context;
        internal DbSet<TEntity> DbSet;

        public Repository(TrinityContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.ToList();
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual TEntity Insert(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
