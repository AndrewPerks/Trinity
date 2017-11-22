using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trinity.DataAccess.Interfaces;
using Trinity.Model;
using Trinity.Services.Interfaces;

namespace Trinity.Services.Concrete
{
    /// <summary>
    /// Client note service that carries out the business transactions involving client notes
    /// </summary>
    public class ClientNoteService : IClientNoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientNoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ClientNote> Get(Expression<Func<ClientNote, bool>> predicate = null, string includeProperties = "")
        {
            var clientNotes = _unitOfWork.Repository<ClientNote>().Get(predicate, includeProperties);
            return clientNotes.ToList();
        }

        public ClientNote GetById(int id)
        {
            var clientNote = _unitOfWork.Repository<ClientNote>().GetById(id);
            return clientNote;
        }

        public void AddClientNote(ClientNote clientNote)
        {
            _unitOfWork.Repository<ClientNote>().Insert(clientNote);
            _unitOfWork.Save();
        }

        public void UpdateClientNote(ClientNote clientNote)
        {
            _unitOfWork.Repository<ClientNote>().Update(clientNote);
            _unitOfWork.Save();
        }

        public void DeleteClientNote(ClientNote clientNote)
        {
            _unitOfWork.Repository<ClientNote>().Delete(clientNote);
            _unitOfWork.Save();
        }

        public void DeleteClientNoteById(int id)
        {
            _unitOfWork.Repository<ClientNote>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
