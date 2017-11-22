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
    public class ContactServiceTests
    {
        private List<Contact> ContactList;
        private Mock<IRepository<Contact>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IContactService _contactService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<Contact>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _contactService = new ContactService(_mockUnitWork.Object);
            ContactList = GenerateContactList().ToList();
            _mockUnitWork.Setup(m => m.Repository<Contact>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetContacts()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Contact, bool>>>(), "")).Returns(ContactList);

            //Act
            List<Contact> results = _contactService.Get().ToList() as List<Contact>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Contact, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetTrinityContacts()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Contact, bool>>>(), "")).Returns(ContactList);

            //Act
            List<Contact> results = _contactService.GetTrinityUsers().ToList() as List<Contact>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Contact, bool>>>(), ""), Times.Once);
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => ContactList.Single(x => x.Id == i));

            //Act
            Contact contact = _contactService.GetById(1);

            //Assert
            Assert.IsNotNull(contact);
            Assert.AreEqual(ContactList.FirstOrDefault(), contact);
        }

        [TestMethod]
        public void CanGetByUsername()
        {
            // Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Contact, bool>>>(), ""))
                .Returns(ContactList.Where(x => x.Username == "admin"));

            //Act
            Contact contact = _contactService.GetByUsername("admin");

            //Assert
            Assert.IsNotNull(contact);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Contact, bool>>>(), ""), Times.Once);
            Assert.AreEqual(ContactList.FirstOrDefault(), contact);
        }

        [TestMethod]
        public void CanAddContact()
        {
            int Id = 1;
            Contact contact = new Contact(){Id = 1, LastName = "New Contact"};
            _mockRepository.Setup(m => m.Insert(contact)).Returns((Contact returnContact) =>
            {
                returnContact.Id = Id;
                return contact;
            });

            //Act
            _contactService.AddContact(contact);

            //Assert
            Assert.AreEqual(Id, contact.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateContact()
        {
            //Arrange
            string lastName = "Updated Contact";
            Contact contact = new Contact() { Id = 1, LastName = "New Contact" };
            _mockRepository.Setup(m => m.Update(contact)).Callback((Contact returnContact) =>
            {
                returnContact.LastName = lastName;
            });

            //Act
            _contactService.UpdateContact(contact);

            //Assert
            Assert.AreEqual(lastName, contact.LastName);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(contact), Times.Once());
        }

        [TestMethod]
        public void CanDeleteContact()
        {
            //Arrange
            Contact contact = new Contact() { Id = 1, LastName = "New Contact" };

            //Act
            _contactService.DeleteContact(contact);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(contact), Times.Once());
        }

        [TestMethod]
        public void CanDeleteContactById()
        {
            //Arrange
            Contact contact = new Contact() { Id = 1, LastName = "New Contact" };

            //Act
            _contactService.DeleteContactById(contact.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(contact.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _contactService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<Contact> GenerateContactList()
        {
            var contacts = new List<Contact>
            {
                //Add attributes when needed
                new Contact {Id = 1, Username = "admin", Client_Id = 1, LastName = "Perkins", Roles = new Collection<Role>(new[] {new Role{ Id = 3, Role1 = "Trinity"}}) },
                new Contact {Id = 2, Username = "user", Client_Id = 1, LastName = "Test"}

            }.AsQueryable();
            return contacts.ToList();
        }

    }
}
