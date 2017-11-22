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
    public class TitleServiceTests
    {
        private List<Title> TitleList;
        private Mock<IRepository<Title>> _mockRepository;
        private Mock<IUnitOfWork> _mockUnitWork;
        private ITitleService _titleService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository<Title>>();
            _mockUnitWork = new Mock<IUnitOfWork>();
            _titleService = new TitleService(_mockUnitWork.Object);
            TitleList = GenerateTitleList().ToList();
            _mockUnitWork.Setup(m => m.Repository<Title>()).Returns(_mockRepository.Object);
        }

        [TestMethod]
        public void CanGetAddresses()
        {
            //Arrange
            _mockRepository.Setup(x => x.Get(It.IsAny<Expression<Func<Title, bool>>>(), "")).Returns(TitleList);

            //Act
            List<Title> results = _titleService.Get().ToList() as List<Title>;

            //Assert
            Assert.IsNotNull(results);
            _mockRepository.Verify(x => x.Get(It.IsAny<Expression<Func<Title, bool>>>(), ""), Times.Once);
            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        public void Test_Dispose()
        {
            //Arrange

            //Act
            _titleService.Dispose();

            //Assert
            _mockUnitWork.Verify(x => x.Dispose(), Times.Once);
        }

        private List<Title> GenerateTitleList()
        {
            var titles = new List<Title>
            {
                //Add attributes when needed
                new Title {Id = 1, Name = "Mr"},                
                new Title {Id = 2, Name = "Mrs"}

            }.AsQueryable();
            return titles.ToList();
        }

    }
}
