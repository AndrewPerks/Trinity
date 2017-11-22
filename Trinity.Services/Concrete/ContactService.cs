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
    /// Contact service that carries out the business transactions involving contacts
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Contact> Get(Expression<Func<Contact, bool>> predicate = null, string includeProperties = "")
        {
            var contacts = _unitOfWork.Repository<Contact>().Get(predicate, includeProperties);
            return contacts.ToList();
        }

        public List<Contact> GetTrinityUsers()
        {
            var contacts = Get(x => x.Deleted == false);
            var trinityUsers = contacts.Where(x => x.Roles.Any(c => c.Role1 == "Trinity"));
            return trinityUsers.ToList();
        }

        public List<Contact> GetByClientId(int id)
        {
            var contacts = Get(x => x.Client.Id == id && x.Deleted == false);
            return contacts.ToList();
        }

        public Contact GetById(int id)
        {
            var contact = _unitOfWork.Repository<Contact>().GetById(id);
            return contact;
        }

        public Contact GetByUsername(string username)
        {
            var contact = _unitOfWork.Repository<Contact>().Get(c => c.Username == username).SingleOrDefault();
            return contact;
        }

        public void AddContact(Contact contact)
        {
            _unitOfWork.Repository<Contact>().Insert(contact);
            _unitOfWork.Save();
        }

        public void UpdateContact(Contact contact)
        {
            _unitOfWork.Repository<Contact>().Update(contact);
            _unitOfWork.Save();
        }

        public void DeleteContact(Contact contact)
        {
            _unitOfWork.Repository<Contact>().Delete(contact);
            _unitOfWork.Save();
        }

        public void DeleteContactById(int id)
        {
            _unitOfWork.Repository<Contact>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
