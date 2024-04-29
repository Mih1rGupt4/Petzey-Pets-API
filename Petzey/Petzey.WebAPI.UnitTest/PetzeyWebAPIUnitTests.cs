using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Petzey.Domain.Interfaces;
using Petzey.Domain.Entities;
using System.Collections.Generic;
using Petzey.WebAPI.Controllers;
using Petzey.Data.Repository;
using System.Web.Http;
using System.Web.Http.Results;
using System.Linq;

namespace Petzey.WebAPI.UnitTest
{
    [TestClass]
    public class PetzeyWebAPIUnitTests
    {
        [TestMethod]
        public void OkOrNotFound_Returns_NotFound_When_Object_Is_Null()
        {
            // Arrange
            var controller = new PetsController();

            // Act
            IHttpActionResult actionResult = controller.OkOrNotFound(null);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void OkOrNotFound_Returns_Ok_When_Object_Is_Not_Null()
        {
            // Arrange
            var controller = new PetsController();
            var testObject = new object(); // You can replace this with any object

            // Act
            IHttpActionResult actionResult = controller.OkOrNotFound(testObject);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkNegotiatedContentResult<object>));
        }

        [TestMethod]
        public async Task GetPetDetailsByPetID_Returns_Ok_With_Pet()
        {
            // Arrange
            int petId = 1;
            var mock = new Mock<IPetsRepository>();
            Pet p = new Pet
            {
                PetID = petId,
                PetName = "Fido",
                Age = "3",
                Gender = "Male",
                PetParentID = 1,
                Allergies = "",
                BloodGroup = "1",
                Breed = "asdf",
                DateOfBirth = new DateTime(),
                LastAppointmentDate = new DateTime(),
                Species = "asfd",
                PetImage = new byte[10]
            };
            mock.Setup(repo => repo.GetPetDetailsByPetIDAsync(petId)).ReturnsAsync(p);

            var controller = new PetsController(mock.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetPetDetailsByPetID(petId);
            var contentResult = actionResult as OkNegotiatedContentResult<object>;

            // Assert
            Assert.IsNotNull(contentResult);

            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(petId, ((Pet)contentResult.Content).PetID);

            // Verify that OkOrNotFound is called with the correct parameter
            mock.Verify(repo => repo.GetPetDetailsByPetIDAsync(petId), Times.Once);

        }

        [TestMethod]
        public async Task GetPetDetailsByPetID_Returns_NotFound_When_Pet_Not_Found()
        {
            // Arrange
            int petId = 1;
            var mock = new Mock<IPetsRepository>();
            mock.Setup(repo => repo.GetPetDetailsByPetIDAsync(petId)).ReturnsAsync((Pet)null);
            var controller = new PetsController(mock.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetPetDetailsByPetID(petId);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));

            // Verify that OkOrNotFound is called with the correct parameter
            mock.Verify(repo => repo.GetPetDetailsByPetIDAsync(petId), Times.Once);
        }


        [TestMethod]
        public async Task GetAllPets_WithPets_ReturnOk()
        {
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetAllPetsAsync()).ReturnsAsync(new List<Pet> { new Pet { PetID = 1, PetParentID = 2, Species = "Dog", Breed = "Pug", PetName = "name_test", Age = "2" } });

            var controller = new PetsController(mockRepo.Object);
            var result = await controller.GetAllPets();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<Pet>>));

            var contentResult = result as OkNegotiatedContentResult<List<Pet>>;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count);
            Assert.AreEqual("name_test", contentResult.Content.First().PetName);
            Assert.AreEqual(1, contentResult.Content.First().PetID);
            Assert.AreEqual(2, contentResult.Content.First().PetParentID);
            Assert.AreEqual("Dog", contentResult.Content.First().Species);
            Assert.AreEqual("Pug", contentResult.Content.First().Breed);
            Assert.AreEqual("2", contentResult.Content.First().Age);

        }

        [TestMethod]
        public async Task GetAllPets_WithoutPets_ReturnsBadRequest()
        {
            //Arrange
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetAllPetsAsync()).ReturnsAsync(new List<Pet>());
            var controller = new PetsController(mockRepo.Object);

            // Act
            var result = await controller.GetAllPets();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void AddPets_WhenPetIsAdded_ReturnsOk()
        {
            //Arrange
            var mockRepo = new Mock<IPetsRepository>();
            Pet test_pet = new Pet
            {
                PetID = 1,
                PetParentID = 1001,
                PetName = "Fluffy",
                PetImage = new byte[0],
                Species = "Dog",
                Breed = "Labrador Retriever",
                Gender = "Male",
                DateOfBirth = new DateTime(2019, 5, 15),
                Age = "5 years",
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            mockRepo.Setup(repo => repo.AddPet(It.IsAny<Pet>())).ReturnsAsync(test_pet);
            var controller = new PetsController(mockRepo.Object);

            //Act
            IHttpActionResult result = controller.AddPet(test_pet);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));

        }

        [TestMethod]
        public void AddPets_WhenPetIsNotAdded_ReturnsBadRequest()
        {
            //Arrange
            Pet test_pet = new Pet
            {
                PetID = 1,
                PetParentID = 1001,
                PetName = "Fluffy",
                PetImage = new byte[0],
                Species = "Dog",
                Breed = "Labrador Retriever",
                Gender = "Male",
                DateOfBirth = new DateTime(2019, 5, 15),
                Age = "5 years",
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.AddPet(It.IsAny<Pet>())).ReturnsAsync((Pet)null); // Simulate failed addition

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = controller.AddPet(test_pet);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void EditPet_WhenPetDataIsEdited_ReturnsOk()
        {
            var mockRepo = new Mock<IPetsRepository>();
            Pet test_pet = new Pet
            {
                PetID = 1,
                PetParentID = 1001,
                PetName = "Fluffy_edited",
                PetImage = new byte[0],
                Species = "Dog",
                Breed = "Labrador Retriever",
                Gender = "Male",
                DateOfBirth = new DateTime(2019, 5, 15),
                Age = "5 years",
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            mockRepo.Setup(repo => repo.EditPet(It.IsAny<Pet>())).ReturnsAsync(test_pet);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = controller.EditPet(test_pet);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void EditPet_WhenPetDataEditFails_ReturnsBadRequest()
        {
            var mockRepo = new Mock<IPetsRepository>();
            Pet test_pet = new Pet
            {
                PetID = 1,
                PetParentID = 1001,
                PetName = "Fluffy_edited",
                PetImage = new byte[0],
                Species = "Dog",
                Breed = "Labrador Retriever",
                Gender = "Male",
                DateOfBirth = new DateTime(2019, 5, 15),
                Age = "5 years",
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            mockRepo.Setup(repo => repo.EditPet(It.IsAny<Pet>())).ReturnsAsync((Pet)null); // Simulate failed edit

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = controller.EditPet(test_pet);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task AddLastAppointmentDate_WhenAddingDateIsSuccessful_ReturnsOk()
        {
            // Arrange
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.AddLastAppointmentDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(true);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.addLastAppointmentDate(1, DateTime.Now);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public async Task AddLastAppointmentDate_WhenAddingDateIsUnsuccessful_ReturnsBadRequest()
        {
            // Arrange
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.AddLastAppointmentDate(It.IsAny<DateTime>(), It.IsAny<int>())).ReturnsAsync(false);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.addLastAppointmentDate(1, DateTime.Now);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public async Task DeletePet_WhenPetIsDeleted_ReturnsOk()
        {
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.DeletePetAsync(It.IsAny<int>())).ReturnsAsync(true);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.DeletePet(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
        }

        [TestMethod]
        public async Task DeletePet_WhenPetIsNotFound_ReturnsNotFound()
        {
            // Arrange
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.DeletePetAsync(It.IsAny<int>())).ReturnsAsync(false);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.DeletePet(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetPetsByParentID_WhenPetsAreFound_ReturnsOk()
        {
            // Arrange
            int parentId = 1;
            var mockRepo = new Mock<IPetsRepository>();
            Pet test_pet = new Pet
            {
                PetID = 1,
                PetParentID = parentId,
                PetName = "Fluffy_edited",
                PetImage = new byte[0],
                Species = "Dog",
                Breed = "Labrador Retriever",
                Gender = "Male",
                DateOfBirth = new DateTime(2019, 5, 15),
                Age = "5 years",
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            List<Pet> Pets = new List<Pet> { test_pet };
            mockRepo.Setup(repo => repo.GetPetsByPetParentIdAsync(parentId)).ReturnsAsync(Pets);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.GetPetsByPetParentId(parentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<Pet>>));

            var contentResult = result as OkNegotiatedContentResult<List<Pet>>;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(Pets,contentResult.Content);           
        }


        [TestMethod]
        public async Task GetPetsByParentID_WhenPetsAreNotFound_ReturnsNotFound()
        {
            // Arrange
            int parentId = 1;
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetPetsByPetParentIdAsync(parentId)).ReturnsAsync(new List<Pet>());

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.GetPetsByPetParentId(parentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

    }
}
