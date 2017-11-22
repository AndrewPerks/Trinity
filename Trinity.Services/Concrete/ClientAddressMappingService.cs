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
    /// Client address mapping service that carries out the business transactions involving client address mappings 
    /// </summary>
    public class ClientAddressMappingService : IClientAddressMappingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ClientAddressMappingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Client_Address_Mapping> Get(Expression<Func<Client_Address_Mapping, bool>> predicate = null, string includeProperties = "")
        {
            var clientAddressMappings = _unitOfWork.Repository<Client_Address_Mapping>().Get(predicate, includeProperties);
            return clientAddressMappings.ToList();
        }

        public Client_Address_Mapping GetById(int id)
        {
            var clientAddressMapping = _unitOfWork.Repository<Client_Address_Mapping>().GetById(id);
            return clientAddressMapping;
        }

        public List<Client_Address_Mapping> GetClientAddressMappingsByClientId(int id)
        {
            var clientAddressMappings = Get();
            var clientAddresses = clientAddressMappings.Where(x => x.Client_Id == id);
            return clientAddresses.ToList();
        }

        public void AddClientAddressMapping(Client_Address_Mapping clientAddressMapping)
        {
            _unitOfWork.Repository<Client_Address_Mapping>().Insert(clientAddressMapping);
            _unitOfWork.Save();
        }

        public void UpdateClientAddressMapping(Client_Address_Mapping clientAddressMapping)
        {
            _unitOfWork.Repository<Client_Address_Mapping>().Update(clientAddressMapping);
            _unitOfWork.Save();
        }

        public void DeleteClientAddressMapping(Client_Address_Mapping clientAddressMapping)
        {
            _unitOfWork.Repository<Client_Address_Mapping>().Delete(clientAddressMapping);
            _unitOfWork.Save();
        }

        public void DeleteClientAddressMappingById(int id)
        {
            _unitOfWork.Repository<Client_Address_Mapping>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
