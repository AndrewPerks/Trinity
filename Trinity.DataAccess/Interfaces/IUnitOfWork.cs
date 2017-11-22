using System;
using Trinity.DataAccess.Concrete;

namespace Trinity.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for the UnitOfWork class
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        TrinityContext GetContext();
        IRepository<T> Repository<T>() where T : class;
        void Save();
    }
}
