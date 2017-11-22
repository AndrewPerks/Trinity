using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the client note service
    /// </summary>
    public interface IClientNoteService
    {
        List<ClientNote> Get(Expression<Func<ClientNote, bool>> predicate = null, string includeProperties = "");
        ClientNote GetById(int id);
        void AddClientNote(ClientNote clientNote);
        void UpdateClientNote(ClientNote clientNote);
        void DeleteClientNote(ClientNote clientNote);
        void DeleteClientNoteById(int id);
        void Dispose();
    }
}
