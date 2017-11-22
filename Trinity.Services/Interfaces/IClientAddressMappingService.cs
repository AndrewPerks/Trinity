using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the client address mapping service
    /// </summary>
    public interface IClientAddressMappingService
    {
        List<Client_Address_Mapping> Get(Expression<Func<Client_Address_Mapping, bool>> predicate = null, string includeProperties = "");
        Client_Address_Mapping GetById(int id);
        List<Client_Address_Mapping> GetClientAddressMappingsByClientId(int id);
        void AddClientAddressMapping(Client_Address_Mapping clientAddressMapping);
        void UpdateClientAddressMapping(Client_Address_Mapping clientAddressMapping);
        void DeleteClientAddressMapping(Client_Address_Mapping clientAddressMapping);
        void DeleteClientAddressMappingById(int id);
        void Dispose();
    }
}
