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
    /// Client note category service that carries out the business transactions involving client note categories
    /// </summary>
    public class ClientNoteCategoryService : IClientNoteCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientNoteCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<ClientNoteCategory> Get(Expression<Func<ClientNoteCategory, bool>> predicate = null, string includeProperties = "")
        {
            var clientNoteCategories = _unitOfWork.Repository<ClientNoteCategory>().Get(predicate, includeProperties);
            return clientNoteCategories.ToList();
        }

        public ClientNoteCategory GetById(int id)
        {
            var clientNoteCategory = _unitOfWork.Repository<ClientNoteCategory>().GetById(id);
            return clientNoteCategory;
        }

        public void AddClientNoteCategory(ClientNoteCategory clientNoteCategory)
        {
            _unitOfWork.Repository<ClientNoteCategory>().Insert(clientNoteCategory);
            _unitOfWork.Save();
        }

        public void UpdateClientNoteCategory(ClientNoteCategory clientNoteCategory)
        {
            _unitOfWork.Repository<ClientNoteCategory>().Update(clientNoteCategory);
            _unitOfWork.Save();
        }

        public void DeleteClientNoteCategory(ClientNoteCategory clientNoteCategory)
        {
            _unitOfWork.Repository<ClientNoteCategory>().Delete(clientNoteCategory);
            _unitOfWork.Save();
        }

        public void DeleteClientNoteCategoryById(int id)
        {
            _unitOfWork.Repository<ClientNoteCategory>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
