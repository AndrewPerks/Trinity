using System;
using System.Collections.Generic;
using System.Linq;
using Trinity.DataAccess.Interfaces;

namespace Trinity.DataAccess.Concrete
{
    /// <summary>
    /// Maintains lists of business objects in-memory which have been changed (inserted, updated, or deleted) during a transaction
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TrinityContext _context = new TrinityContext();        
        
        public Dictionary<Type, object> repositories = new Dictionary<Type, object>();

        public TrinityContext GetContext()
        {
            return _context;
        }

        public UnitOfWork(TrinityContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (repositories.Keys.Contains(typeof(T)) == true)
            {
                return repositories[typeof(T)] as IRepository<T>;
            }
            IRepository<T> repo = new Repository<T>(_context);
            repositories.Add(typeof(T), repo);
            return repo;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
