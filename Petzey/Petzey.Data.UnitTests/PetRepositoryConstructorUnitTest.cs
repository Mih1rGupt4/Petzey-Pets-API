using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Petzey.Data.Repository;
using Petzey.Domain.Interfaces;
using System;
using System.Reflection;

namespace Petzey.Data.UnitTests
{
    [TestClass]
    public class PetRepositoryConstructorUnitTest
    {
        private Mock<IPetzeyPetsDbContext> _mockDbContext;
        private PetsRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize mock DbContext and repository for each test
            _mockDbContext = new Mock<IPetzeyPetsDbContext>();
        }

        [TestMethod]
        public void Constructor_WithDbContext_SetsDbContext()
        {
            // Initialize repository with mocked context
            _repository = new PetsRepository(_mockDbContext.Object);
            // Assert
            Assert.IsNotNull(_repository);
            Assert.IsInstanceOfType(_repository, typeof(PetsRepository));
            Assert.IsNotNull(_repository.GetType().GetField("_db", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(_repository));
        }

        [TestMethod]
        public void Constructor_WithoutDbContext_CreatesNewDbContext()
        {
            // Act
            var repository = new PetsRepository();

            // Assert
            Assert.IsNotNull(repository);
            Assert.IsInstanceOfType(repository, typeof(PetsRepository));
            Assert.IsNotNull(repository.GetType().GetField("_db", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(repository));
        }
    }
}
