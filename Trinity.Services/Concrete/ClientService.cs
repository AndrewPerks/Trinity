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
    /// Client service that carries out the business transactions involving clients
    /// </summary>
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Client> Get(Expression<Func<Client, bool>> predicate = null, string includeProperties = "")
        {
            var clients = _unitOfWork.Repository<Client>().Get(predicate, includeProperties);
            return clients.ToList();
        }

        public List<Client> GetActiveClients()
        {
            var clients = Get(x => x.Deleted == false && x.Active);
            return clients.ToList();
        }        

        public List<Client> Search(string clientName, string lastName, string postcode)
        {
            var context = _unitOfWork.GetContext();

            var query = from client in context.Clients
                        where (string.IsNullOrEmpty(clientName) || client.ClientName.Contains(clientName))
                        && (string.IsNullOrEmpty(lastName) || client.Contacts.Any(c => c.LastName.Contains(lastName)))
                        && (string.IsNullOrEmpty(postcode) || client.Address.PostalCode.Contains(postcode) || client.Address1.PostalCode.Contains(postcode) || client.Address2.PostalCode.Contains(postcode))
                        select client;

            return query.ToList();
        }

        public Client GetByClientName(string clientName)
        {
            var client = _unitOfWork.Repository<Client>().Get().SingleOrDefault(x => x.ClientName == clientName);
            return client;
        }

        public Client GetById(int id)
        {
            var client = _unitOfWork.Repository<Client>().GetById(id);
            return client;
        }

        public void AddClient(Client client)
        {
            _unitOfWork.Repository<Client>().Insert(client);
            _unitOfWork.Save();
        }

        public void UpdateClient(Client client)
        {
            _unitOfWork.Repository<Client>().Update(client);
            _unitOfWork.Save();
        }

        public void DeleteClient(Client client)
        {
            _unitOfWork.Repository<Client>().Delete(client);
            _unitOfWork.Save();
        }

        public void DeleteClientById(int id)
        {
            _unitOfWork.Repository<Client>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
