using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Trinity.Model;

namespace Trinity.Services.Interfaces
{
    /// <summary>
    /// Interface for the address service
    /// </summary>
    public interface IAddressService
    {
        List<Address> Get(Expression<Func<Address, bool>> predicate = null, string includeProperties = "");
        Address GetById(int id);
        List<Address> GetAddressesByClientId(List<Client_Address_Mapping> mappings);
        void AddAddress(Address address);
        void UpdateAddress(Address address);
        void DeleteAddress(Address address);
        void DeleteAddressById(int id);
        void Dispose();
    }
}
