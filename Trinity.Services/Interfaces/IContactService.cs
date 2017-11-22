using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the contact service
    /// </summary>
    public interface IContactService
    {
        List<Contact> Get(Expression<Func<Contact, bool>> predicate = null, string includeProperties = "");
        List<Contact> GetTrinityUsers();
        List<Contact> GetByClientId(int id);
        Contact GetById(int id);
        Contact GetByUsername(string username);
        void AddContact(Contact contact);
        void UpdateContact(Contact contact);
        void DeleteContact(Contact contact);
        void DeleteContactById(int id);
        void Dispose();
    }
}
