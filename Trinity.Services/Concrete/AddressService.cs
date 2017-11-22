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
    /// Address service that carries out the business transactions involving addresses
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Address> Get(Expression<Func<Address, bool>> predicate = null, string includeProperties = "")
        {
            var addresses = _unitOfWork.Repository<Address>().Get(predicate, includeProperties);
            return addresses.ToList();
        }

        public Address GetById(int id)
        {
            var address = _unitOfWork.Repository<Address>().GetById(id);
            return address;
        }

        public List<Address> GetAddressesByClientId(List<Client_Address_Mapping> mappings)
        {
            var addresses = new List<Address>();
            foreach (var clientAddressMapping in mappings)
            {
                addresses.Add(GetById(clientAddressMapping.Address_Id));
            }
            return addresses;           
        }

        public void AddAddress(Address address)
        {
            _unitOfWork.Repository<Address>().Insert(address);
            _unitOfWork.Save();
        }

        public void UpdateAddress(Address address)
        {
            _unitOfWork.Repository<Address>().Update(address);
            _unitOfWork.Save();
        }

        public void DeleteAddress(Address address)
        {
            _unitOfWork.Repository<Address>().Delete(address);
            _unitOfWork.Save();
        }

        public void DeleteAddressById(int id)
        {
            _unitOfWork.Repository<Address>().Delete(id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
