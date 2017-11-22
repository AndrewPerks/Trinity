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
    public class RoleServiceTests
    {
        private List<Role> RoleList;
        private Mock<IRepository<Role>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IRoleService _roleService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<Role>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _roleService = new RoleService(_mockUnitWork.Object);
            RoleList = GenerateRoleList().ToList();
            _mockUnitWork.Setup(m => m.Repository<Role>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetRoles()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>(), "")).Returns(RoleList);

            //Act
            List<Role> results = _roleService.Get().ToList() as List<Role>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => RoleList.Single(x => x.Id == i));

            //Act
            Role role = _roleService.GetById(1);

            //Assert
            Assert.IsNotNull(role);
            Assert.AreEqual(RoleList.FirstOrDefault(), role);
        }

        [TestMethod]
        public void CanSearch()
        {
            // Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>(), ""))
                .Returns(RoleList.Where(x => x.Role1 == "User"));

            //Act
            Role role = _roleService.Search("User");

            //Assert
            Assert.IsNotNull(role);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Role, bool>>>(), ""), Times.Once);
            Assert.AreEqual(role.Role1, "User");
        }

        [TestMethod]
        public void CanAddRole()
        {
            int Id = 1;
            Role role = new Role() { Id = 1, Role1 = "New Role" };
            _mockRepository.Setup(m => m.Insert(role)).Returns((Role returnRole) =>
            {
                returnRole.Id = Id;
                return role;
            });

            //Act
            _roleService.AddRole(role);

            //Assert
            Assert.AreEqual(Id, role.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateRole()
        {
            //Arrange
            string roleName = "Updated Role";
            Role role = new Role() { Id = 1, Role1 = "New Role" };
            _mockRepository.Setup(m => m.Update(role)).Callback((Role returnRole) =>
            {
                returnRole.Role1 = roleName;
            });

            //Act
            _roleService.UpdateRole(role);

            //Assert
            Assert.AreEqual(roleName, role.Role1);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(role), Times.Once());
        }

        [TestMethod]
        public void CanDeleteRole()
        {
            //Arrange
            Role role = new Role() { Id = 1, Role1 = "New Role" };

            //Act
            _roleService.DeleteRole(role);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(role), Times.Once());
        }

        [TestMethod]
        public void CanDeleteRoleById()
        {
            //Arrange
            Role role = new Role() { Id = 1, Role1 = "New Role" };

            //Act
            _roleService.DeleteRoleById(role.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(role.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _roleService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<Role> GenerateRoleList()
        {
            var roles = new List<Role>
            {
                //Add attributes when needed
                new Role {Id = 1, Role1 = "Administrator"},                
                new Role {Id = 2, Role1 = "User"}      

            }.AsQueryable();
            return roles.ToList();
        }

    }
}
