//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using Petzey.Data.Repository;
//using Petzey.Domain.Entities;
//using Petzey.Domain.Interfaces;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Threading.Tasks;
//using System;
//using System.Text;

//namespace Petzey.Data.UnitTests
//{
//    /// <summary>
//    /// Summary description for PetsRepositoryTests
//    /// </summary>
//    [TestClass]
//    public class PetsRepositoryMethodsUnitTests
//    {
//        private Mock<IPetzeyPetsDbContext> _mockDbContext;
//        private PetsRepository _repository;

//        [TestInitialize]
//        public void Initialize()
//        {
//            // Initialize mock DbContext and repository for each test
//            _mockDbContext = new Mock<IPetzeyPetsDbContext>();
//            // Initialize repository with mocked context
//            _repository = new PetsRepository(_mockDbContext.Object);
//        }

//        [TestMethod]
//        public async Task GetPetDetailsByPetIDAsync_Returns_Correct_Pet()
//        {
//            // Arrange
//            var testData = new List<Pet>
//            {
//                new Pet { PetID = 1, PetName = "Fluffy", Species = "Dog", Breed = "Golden Retriever", Gender = "Male", DateOfBirth = new DateTime(2019, 5, 15), Age = "Adult", BloodGroup = "A+", Allergies = "None" },
//                new Pet { PetID = 2, PetName = "Whiskers", Species = "Cat", Breed = "Persian", Gender = "Female", DateOfBirth = new DateTime(2020, 3, 10), Age = "Young", BloodGroup = "O-", Allergies = "None" },
//                new Pet { PetID = 3, PetName = "Buddy", Species = "Dog", Breed = "Labrador Retriever", Gender = "Male", DateOfBirth = new DateTime(2018, 8, 20), Age = "Adult", BloodGroup = "B+", Allergies = "Pollen" }
//            }.AsQueryable();

//            SetupMockDbSet(testData);

//            // Act
//            var pet = await _repository.GetPetDetailsByPetIDAsync(1);

//            // Assert
//            Assert.IsNotNull(pet);
//            Assert.AreEqual(1, pet.PetID);
//            Assert.AreEqual("Fluffy", pet.PetName);
//            Assert.AreEqual("Dog", pet.Species);
//            Assert.AreEqual("Golden Retriever", pet.Breed);
//            Assert.AreEqual("Male", pet.Gender);
//            Assert.AreEqual(new DateTime(2019, 5, 15), pet.DateOfBirth);
//            Assert.AreEqual("Adult", pet.Age);
//            Assert.AreEqual("A+", pet.BloodGroup);
//            Assert.AreEqual("None", pet.Allergies);
//        }

//        [TestMethod]
//        public async Task GetPetsAsync_Returns_Correct_Page()
//        {
//            // Arrange
//            var testData = new List<Pet>
//            {
//                new Pet { PetID = 1, PetName = "Fluffy", Species = "Dog", Breed = "Golden Retriever", Gender = "Male", DateOfBirth = new DateTime(2019, 5, 15), Age = "Adult", BloodGroup = "A+", Allergies = "None" },
//                new Pet { PetID = 2, PetName = "Whiskers", Species = "Cat", Breed = "Persian", Gender = "Female", DateOfBirth = new DateTime(2020, 3, 10), Age = "Young", BloodGroup = "O-", Allergies = "None" },
//                new Pet { PetID = 3, PetName = "Buddy", Species = "Dog", Breed = "Labrador Retriever", Gender = "Male", DateOfBirth = new DateTime(2018, 8, 20), Age = "Adult", BloodGroup = "B+", Allergies = "Pollen" }
//            }.AsQueryable();

//            SetupMockDbSet(testData);

//            // Act
//            var pets = await _repository.GetPetsAsync(1, 2);

//            // Assert
//            Assert.IsNotNull(pets);
//            Assert.AreEqual(2, pets.Count);
//            Assert.AreEqual(1, pets[0].PetID);
//            Assert.AreEqual("Fluffy", pets[0].PetName);
//            Assert.AreEqual("Dog", pets[0].Species);
//            Assert.AreEqual("Golden Retriever", pets[0].Breed);
//            Assert.AreEqual("Male", pets[0].Gender);
//            Assert.AreEqual(new DateTime(2019, 5, 15), pets[0].DateOfBirth);
//            Assert.AreEqual("Adult", pets[0].Age);
//            Assert.AreEqual("A+", pets[0].BloodGroup);
//            Assert.AreEqual("None", pets[0].Allergies);
//            Assert.AreEqual(2, pets[1].PetID);
//            Assert.AreEqual("Whiskers", pets[1].PetName);
//            Assert.AreEqual("Cat", pets[1].Species);
//            Assert.AreEqual("Persian", pets[1].Breed);
//            Assert.AreEqual("Female", pets[1].Gender);
//            Assert.AreEqual(new DateTime(2020, 3, 10), pets[1].DateOfBirth);
//            Assert.AreEqual("Young", pets[1].Age);
//            Assert.AreEqual("O-", pets[1].BloodGroup);
//            Assert.AreEqual("None", pets[1].Allergies);
//        }

//        private void SetupMockDbSet(IQueryable<Pet> testData)
//        {
//            var mockDbSet = new Mock<PetzeyPetsDbContext>();

//            // Convert the test data to a list to be able to use it with DbSet's FromList method
//            var testDataList = testData.ToList();
            
//            // Mock DbSet's asynchronous methods using a list instead of IQueryable
//            var asyncTestData = new List<Pet>(testDataList);

//            mockDbSet.As<IQueryable<Pet>>().Setup(m => m.Provider).Returns(testData.Provider);
//            mockDbSet.As<IQueryable<Pet>>().Setup(m => m.Expression).Returns(testData.Expression);
//            mockDbSet.As<IQueryable<Pet>>().Setup(m => m.ElementType).Returns(testData.ElementType);
//            mockDbSet.As<IQueryable<Pet>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

//        }
//    }
//}
