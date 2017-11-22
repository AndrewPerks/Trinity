using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the client note category service
    /// </summary>
    public interface IClientNoteCategoryService
    {
        List<ClientNoteCategory> Get(Expression<Func<ClientNoteCategory, bool>> predicate = null, string includeProperties = "");
        ClientNoteCategory GetById(int id);
        void AddClientNoteCategory(ClientNoteCategory clientNoteCategory);
        void UpdateClientNoteCategory(ClientNoteCategory clientNoteCategory);
        void DeleteClientNoteCategory(ClientNoteCategory clientNoteCategory);
        void DeleteClientNoteCategoryById(int id);
        void Dispose();
    }
}
