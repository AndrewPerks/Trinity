using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the client service
    /// </summary>
    public interface IClientService
    {
        List<Client> Get(Expression<Func<Client, bool>> predicate = null, string includeProperties = "");
        List<Client> GetActiveClients();
        List<Client> Search(string clientName, string lastName, string postcode);
        Client GetByClientName(string clientName);
        Client GetById(int id);
        void AddClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(Client client);
        void DeleteClientById(int id);
        void Dispose();
    }
}
