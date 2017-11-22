using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the title service
    /// </summary>
    public interface ITitleService
    {
        List<Title> Get(Expression<Func<Title, bool>> predicate = null, string includeProperties = "");
        Title GetById(int id);
        void Dispose();
    }
}
