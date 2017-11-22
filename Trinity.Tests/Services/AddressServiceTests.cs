using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Trinity.DataAccess.Interfaces;
using Trinity.Model;
using Trinity.Services.Concrete;
using Trinity.Services.Interfaces;

namespace Trinity.Tests.Services
{
    [TestClass]
    public class AddressServiceTests
    {
        private List<Address> AddressList;
        private Mock<IRepository<Address>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IAddressService _addressService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<Address>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _addressService = new AddressService(_mockUnitWork.Object);
            AddressList = GenerateAddressList().ToList();
            _mockUnitWork.Setup(m => m.Repository<Address>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetAddresses()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Address, bool>>>(), "")).Returns(AddressList);

            //Act
            List<Address> results = _addressService.Get().ToList() as List<Address>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Address, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => AddressList.Single(x => x.Id == i));

            //Act
            Address address = _addressService.GetById(1);

            //Assert
            Assert.IsNotNull(address);
            Assert.AreEqual(AddressList.FirstOrDefault(), address);
        }

        [TestMethod]
        public void CanAddAddress()
        {
            int Id = 1;
            Address address = new Address(){Id = 1, Address1 = "New Address"};
            _mockRepository.Setup(m => m.Insert(address)).Returns((Address returnAddress) =>
            {
                returnAddress.Id = Id;
                return address;
            });

            //Act
            _addressService.AddAddress(address);

            //Assert
            Assert.AreEqual(Id, address.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateAddress()
        {
            //Arrange
            string addressLine1 = "Updated Address";
            Address address = new Address() { Id = 1, Address1 = "New Address" };
            _mockRepository.Setup(m => m.Update(address)).Callback((Address returnAddress) =>
            {
                returnAddress.Address1 = addressLine1;
            });

            //Act
            _addressService.UpdateAddress(address);

            //Assert
            Assert.AreEqual(addressLine1, address.Address1);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(address), Times.Once());
        }

        [TestMethod]
        public void CanDeleteAddress()
        {
            //Arrange
            Address address = new Address() { Id = 1, LastName = "New Address" };

            //Act
            _addressService.DeleteAddress(address);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(address), Times.Once());
        }

        [TestMethod]
        public void CanDeleteAddressById()
        {
            //Arrange
            Address address = new Address() { Id = 1, Address1 = "New Address" };

            //Act
            _addressService.DeleteAddressById(address.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(address.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _addressService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<Address> GenerateAddressList()
        {
            var addresses = new List<Address>
            {
                //Add attributes when needed
                new Address {Id = 1, Address1 = "10 Downing Street", Address2 = "Westminster", City = "London", PostalCode = "SW1A 2AA"},
                new Address {Id = 2, Address1 = "29 Nuns Moor Road", Address2 = "Fenham", City = "Newcastle", PostalCode = "NE4 9AU"}

            }.AsQueryable();
            return addresses.ToList();
        }

    }
}
