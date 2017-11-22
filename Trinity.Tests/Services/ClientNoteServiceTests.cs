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
    public class ClientNoteServiceTests
    {
        private List<ClientNote> ClientNoteList;
        private Mock<IRepository<ClientNote>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IClientNoteService _clientNoteService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<ClientNote>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _clientNoteService = new ClientNoteService(_mockUnitWork.Object);
            ClientNoteList = GenerateClientNoteList().ToList();
            _mockUnitWork.Setup(m => m.Repository<ClientNote>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetClientNotes()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<ClientNote, bool>>>(), "")).Returns(ClientNoteList);

            //Act
            List<ClientNote> results = _clientNoteService.Get().ToList() as List<ClientNote>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<ClientNote, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => ClientNoteList.Single(x => x.Id == i));

            //Act
            ClientNote clientNote = _clientNoteService.GetById(1);

            //Assert
            Assert.IsNotNull(clientNote);
            Assert.AreEqual(ClientNoteList.FirstOrDefault(), clientNote);
        }

        [TestMethod]
        public void CanAddClientNote()
        {
            int Id = 1;
            ClientNote clientNote = new ClientNote() { Id = 1, NoteTitle = "New Client Note" };
            _mockRepository.Setup(m => m.Insert(clientNote)).Returns((ClientNote returnClientNote) =>
            {
                returnClientNote.Id = Id;
                return clientNote;
            });

            //Act
            _clientNoteService.AddClientNote(clientNote);

            //Assert
            Assert.AreEqual(Id, clientNote.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateClientNote()
        {
            //Arrange
            string title = "Updated Client Note";
            ClientNote clientNote = new ClientNote() { Id = 1, NoteTitle = "New Client Note" };
            _mockRepository.Setup(m => m.Update(clientNote)).Callback((ClientNote returnClientNote) =>
            {
                returnClientNote.NoteTitle = title;
            });

            //Act
            _clientNoteService.UpdateClientNote(clientNote);

            //Assert
            Assert.AreEqual(title, clientNote.NoteTitle);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(clientNote), Times.Once());
        }

        [TestMethod]
        public void CanDeleteClientNote()
        {
            //Arrange
            ClientNote clientNote = new ClientNote() { Id = 1, NoteTitle = "New Client Note" };

            //Act
            _clientNoteService.DeleteClientNote(clientNote);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(clientNote), Times.Once());
        }

        [TestMethod]
        public void CanDeleteClientNoteById()
        {
            //Arrange
            ClientNote clientNote = new ClientNote() { Id = 1, NoteTitle = "New Client Note" };

            //Act
            _clientNoteService.DeleteClientNoteById(clientNote.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(clientNote.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _clientNoteService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<ClientNote> GenerateClientNoteList()
        {
            var clientNotes = new List<ClientNote>
            {
                //Add attributes when needed
                new ClientNote {Id = 1, NoteTitle = "A Note", NoteContent = "Test note content"},
                new ClientNote {Id = 2, NoteTitle = "Another Note", NoteContent = "More test note content"}

            }.AsQueryable();
            return clientNotes.ToList();
        }

    }
}
