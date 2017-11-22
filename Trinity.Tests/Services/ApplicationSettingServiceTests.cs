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
    public class ApplicationSettingServiceTests
    {
        private List<ApplicationSetting> ApplicationSettingList;
        private Mock<IRepository<ApplicationSetting>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private IApplicationSettingService _applicationSettingService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<ApplicationSetting>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _applicationSettingService = new ApplicationSettingService(_mockUnitWork.Object);
            ApplicationSettingList = GenerateApplicationSettingList().ToList();
            _mockUnitWork.Setup(m => m.Repository<ApplicationSetting>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetApplicationSettings()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<ApplicationSetting, bool>>>(), "")).Returns(ApplicationSettingList);

            //Act
            List<ApplicationSetting> results = _applicationSettingService.Get().ToList() as List<ApplicationSetting>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<ApplicationSetting, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void CanGetById()
        {
            // Arrange
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int i) => ApplicationSettingList.Single(x => x.Id == i));

            //Act
            ApplicationSetting applicationSetting = _applicationSettingService.GetById(1);

            //Assert
            Assert.IsNotNull(applicationSetting);
            Assert.AreEqual(ApplicationSettingList.FirstOrDefault(), applicationSetting);
        }

        [TestMethod]
        public void CanGetTimeoutLength()
        {
            // Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<ApplicationSetting, bool>>>(), ""))
                .Returns(ApplicationSettingList.Where(x => x.ApplicationSettingName == "TimeoutLength"));

            //Act
            ApplicationSetting applicationSetting = _applicationSettingService.GetTimeOutLength();

            //Assert
            Assert.IsNotNull(applicationSetting);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<ApplicationSetting, bool>>>(), ""), Times.Once);
            Assert.AreEqual(ApplicationSettingList.FirstOrDefault(), applicationSetting);
        }

        [TestMethod]
        public void CanAddApplicationSetting()
        {
            int Id = 1;
            ApplicationSetting applicationSetting = new ApplicationSetting() { Id = 1, ApplicationSettingName = "New Application Setting", Value = "New Value"};
            _mockRepository.Setup(m => m.Insert(applicationSetting)).Returns((ApplicationSetting returnApplicationSetting) =>
            {
                returnApplicationSetting.Id = Id;
                return applicationSetting;
            });

            //Act
            _applicationSettingService.AddApplicationSetting(applicationSetting);

            //Assert
            Assert.AreEqual(Id, applicationSetting.Id);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
        }

        [TestMethod]
        public void CanUpdateApplicationSetting()
        {
            //Arrange
            string applicationSettingName = "Updated Application Setting";
            ApplicationSetting applicationSetting = new ApplicationSetting() { Id = 1, ApplicationSettingName = "New Application Setting" };
            _mockRepository.Setup(m => m.Update(applicationSetting)).Callback((ApplicationSetting returnApplicationSetting) =>
            {
                returnApplicationSetting.ApplicationSettingName = applicationSettingName;
            });

            //Act
            _applicationSettingService.UpdateApplicationSetting(applicationSetting);

            //Assert
            Assert.AreEqual(applicationSettingName, applicationSetting.ApplicationSettingName);
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Update(applicationSetting), Times.Once());
        }

        [TestMethod]
        public void CanDeleteApplicationSetting()
        {
            //Arrange
            ApplicationSetting applicationSetting = new ApplicationSetting() { Id = 1, ApplicationSettingName = "New Application Setting" };

            //Act
            _applicationSettingService.DeleteApplicationSetting(applicationSetting);

            //Assert
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(applicationSetting), Times.Once());
        }

        [TestMethod]
        public void CanDeleteApplicationSettingById()
        {
            //Arrange
            ApplicationSetting applicationSetting = new ApplicationSetting() { Id = 1, ApplicationSettingName = "New Application Setting" };

            //Act
            _applicationSettingService.DeleteApplicationSettingById(applicationSetting.Id);

            //Assert           
            _mockUnitWork.Verify(m => m.Save(), Times.Once());
            _mockRepository.Verify(m => m.Delete(applicationSetting.Id), Times.Once());
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _applicationSettingService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<ApplicationSetting> GenerateApplicationSettingList()
        {
            var applicationSettings = new List<ApplicationSetting>
            {
                //Add attributes when needed
                new ApplicationSetting {Id = 1, ApplicationSettingName = "TimeoutLength", Value = "2"},
                new ApplicationSetting {Id = 2, ApplicationSettingName = "Application Setting", Value = "10"}

            }.AsQueryable();
            return applicationSettings.ToList();
        }

    }
}
