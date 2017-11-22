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
    public class ClientServiceTests
    {
        private List<Client> ClientList;
        private Mock<IRepository<Client>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IClientService _clientService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<Client>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _clientService = new ClientService(_mockUnitWork.Object);
            ClientList = GenerateClientList().ToList();
            _mockUnitWork.Setup(m => m.Repository<Client>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetClients()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Client, bool>>>(), "")).Returns(ClientList);

            //Act
            List<Client> results = _clientService.Get().ToList() as List<Client>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Client, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetActiveClients()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Client, bool>>>(), ""))
                .Returns(ClientList.Where(x => x.Deleted == false && x.Active));

            //Act
            List<Client> results = _clientService.GetActiveClients().ToList() as List<Client>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Client, bool>>>(), ""), Times.Once);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void CanGetByClientName()
        {
            // Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Client, bool>>>(), ""))
                .Returns(ClientList);

            //Act
            Client client = _clientService.GetByClientName("Client Name");

            //Assert
            Assert.IsNotNull(client);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Client, bool>>>(), ""), Times.Once);
            Assert.AreEqual("Client Name", client.ClientName);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => ClientList.Single(x => x.Id == i));

            //Act
            Client client = _clientService.GetById(1);

            //Assert
            Assert.IsNotNull(client);
            Assert.AreEqual(ClientList.FirstOrDefault(), client);
        }

        [TestMethod]
        public void CanAddClient()
        {
            int Id = 1;
            Client client = new Client() { Id = 1, ClientName = "New Client" };
            _mockRepository.Setup(m => m.Insert(client)).Returns((Client returnClient) =>
            {
                returnClient.Id = Id;
                return client;
            });

            //Act
            _clientService.AddClient(client);

            //Assert
            Assert.AreEqual(Id, client.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateClient()
        {
            //Arrange
            string clientName = "Updated Client";
            Client client = new Client() { Id = 1, ClientName = "New Client" };
            _mockRepository.Setup(m => m.Update(client)).Callback((Client returnClient) =>
            {
                returnClient.ClientName = clientName;
            });

            //Act
            _clientService.UpdateClient(client);

            //Assert
            Assert.AreEqual(clientName, client.ClientName);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(client), Times.Once());
        }

        [TestMethod]
        public void CanDeleteClient()
        {
            //Arrange
            Client client = new Client() { Id = 1, ClientName = "New Client" };

            //Act
            _clientService.DeleteClient(client);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(client), Times.Once());
        }

        [TestMethod]
        public void CanDeleteClientById()
        {
            //Arrange
            Client client = new Client() { Id = 1, ClientName = "New Client" };

            //Act
            _clientService.DeleteClientById(client.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(client.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _clientService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<Client> GenerateClientList()
        {
            var clients = new List<Client>
            {
                //Add attributes when needed
                new Client {Id = 1, ClientName = "Select Information Systems Ltd", Deleted = true},                
                new Client {Id = 2, ClientName = "Client Name", Active = true},

            }.AsQueryable();
            return clients.ToList();
        }

    }
}
