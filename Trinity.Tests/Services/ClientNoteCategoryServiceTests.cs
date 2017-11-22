using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ClientNoteCategoryServiceTests
    {
        private List<ClientNoteCategory> ClientNoteCategoryList;
        private Mock<IRepository<ClientNoteCategory>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IClientNoteCategoryService _clientNoteCategoryService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<ClientNoteCategory>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _clientNoteCategoryService = new ClientNoteCategoryService(_mockUnitWork.Object);
            ClientNoteCategoryList = GenerateClientNoteCategoryList().ToList();
            _mockUnitWork.Setup(m => m.Repository<ClientNoteCategory>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetClientNoteCategories()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<ClientNoteCategory, bool>>>(), "")).Returns(ClientNoteCategoryList);

            //Act
            List<ClientNoteCategory> results = _clientNoteCategoryService.Get().ToList() as List<ClientNoteCategory>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<ClientNoteCategory, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => ClientNoteCategoryList.Single(x => x.Id == i));

            //Act
            ClientNoteCategory clientNoteCategory = _clientNoteCategoryService.GetById(1);

            //Assert
            Assert.IsNotNull(clientNoteCategory);
            Assert.AreEqual(ClientNoteCategoryList.FirstOrDefault(), clientNoteCategory);
        }

        [TestMethod]
        public void CanAddClientNoteCategory()
        {
            int Id = 1;
            ClientNoteCategory clientNoteCategory = new ClientNoteCategory() { Id = 1, Name = "New Client Note Category" };
            _mockRepository.Setup(m => m.Insert(clientNoteCategory)).Returns((ClientNoteCategory returnClientNoteCategory) =>
            {
                returnClientNoteCategory.Id = Id;
                return clientNoteCategory;
            });

            //Act
            _clientNoteCategoryService.AddClientNoteCategory(clientNoteCategory);

            //Assert
            Assert.AreEqual(Id, clientNoteCategory.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateClientNoteCategory()
        {
            //Arrange
            string name = "Updated Client Note Category";
            ClientNoteCategory clientNoteCategory = new ClientNoteCategory() { Id = 1, Name = "New Client Note Category" };
            _mockRepository.Setup(m => m.Update(clientNoteCategory)).Callback((ClientNoteCategory returnClientNoteCategory) =>
            {
                returnClientNoteCategory.Name = name;
            });

            //Act
            _clientNoteCategoryService.UpdateClientNoteCategory(clientNoteCategory);

            //Assert
            Assert.AreEqual(name, clientNoteCategory.Name);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(clientNoteCategory), Times.Once());
        }

        [TestMethod]
        public void CanDeleteClientNoteCategory()
        {
            //Arrange
            ClientNoteCategory clientNoteCategory = new ClientNoteCategory() { Id = 1, Name = "New Client Note Category" };

            //Act
            _clientNoteCategoryService.DeleteClientNoteCategory(clientNoteCategory);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(clientNoteCategory), Times.Once());
        }

        [TestMethod]
        public void CanDeleteClientNoteCategoryById()
        {
            //Arrange
            ClientNoteCategory clientNoteCategory = new ClientNoteCategory() { Id = 1, Name = "New Client Note Category" };

            //Act
            _clientNoteCategoryService.DeleteClientNoteCategoryById(clientNoteCategory.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(clientNoteCategory.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _clientNoteCategoryService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<ClientNoteCategory> GenerateClientNoteCategoryList()
        {
            var clientNoteCategories = new List<ClientNoteCategory>
            {
                //Add attributes when needed
                new ClientNoteCategory {Id = 1, Name = "Support Contracts", ClientNotes = new Collection<ClientNote>(new[] {new ClientNote{ Id = 1, NoteTitle = "Note"}})},
                new ClientNoteCategory {Id = 2, Name = "Software"},

            }.AsQueryable();
            return clientNoteCategories.ToList();
        }

    }
}
