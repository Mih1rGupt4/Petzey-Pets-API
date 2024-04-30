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
using Petzey.Domain.Dtos;

namespace Petzey.WebAPI.UnitTest
{
    [TestClass]
    public class PetzeyWebAPIUnitTests
    {
        [TestMethod]
        public void OkOrNotFound_Returns_NotFound_When_Object_Is_Null()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository());

            // Act
            IHttpActionResult actionResult = controller.OkOrNotFound(null);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public void OkOrNotFound_Returns_Ok_When_Object_Is_Not_Null()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository());
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
            mockRepo.Setup(repo => repo.GetAllPetsAsync()).ReturnsAsync(new List<Pet> { new Pet { PetID = 1, PetParentID = 2, Species = "Dog", Breed = "Pug", PetName = "name_test"} });

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
        public async Task AddPets_WhenPetIsAdded_ReturnsOk()
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
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            mockRepo.Setup(repo => repo.AddPet(It.IsAny<Pet>())).ReturnsAsync(test_pet);
            var controller = new PetsController(mockRepo.Object);

            //Act
            IHttpActionResult result = await controller.AddPet(test_pet);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<String>));

        }

        [TestMethod]
        public async Task AddPets_WhenPetIsNotAdded_ReturnsBadRequest()
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
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.AddPet(It.IsAny<Pet>())).ReturnsAsync((Pet)null); // Simulate failed addition

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.AddPet(test_pet);

            // Assert
            //Assert.IsNotNull(result);
            //Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));

        }

        [TestMethod]
        public async Task EditPet_WhenPetDataIsEdited_ReturnsOk()
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
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            mockRepo.Setup(repo => repo.EditPet(test_pet)).ReturnsAsync(test_pet);

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.EditPet(test_pet);
            var content = result as OkNegotiatedContentResult<string>;
            // Assert
            Assert.IsNotNull(content);
            Assert.IsNotNull(content.Content);
            Assert.IsInstanceOfType(content, typeof(OkNegotiatedContentResult<string>));
        }

        [TestMethod]
        public async Task EditPet_WhenPetDataEditFails_ReturnsBadRequest()
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
                BloodGroup = "A+",
                Allergies = "None",
                LastAppointmentDate = DateTime.Now.AddDays(-30)
            };
            mockRepo.Setup(repo => repo.EditPet(test_pet)).ReturnsAsync((Pet)null); // Simulate failed edit

            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult result = await controller.EditPet(test_pet);
            // Assert
            //Assert.IsNotNull(content);
            //Assert.IsNotNull(content.Message);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
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
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
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
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
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

        [TestMethod]
        public async Task ConvertPetsToCardPetDetailsDto_Converts_Pets_To_CardPetDetailsDto()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository()); // No need for a mock in this case
            var pets = new List<Pet>
    {
        new Pet { PetID = 1, PetName = "Fido", Gender = "Male", PetParentID = 1, PetImage = new byte[0] },
        new Pet { PetID = 2, PetName = "Fluffy", Gender = "Female", PetParentID = 2, PetImage = new byte[0] }
        // Add more sample pets if needed
    };

            // Act
            var result = await controller.ConvertPetsToCardPetDetailsDtoAsync(pets);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(pets.Count, result.Count);

            for (int i = 0; i < pets.Count; i++)
            {
                Assert.AreEqual(pets[i].PetID, result[i].PetID);
                Assert.AreEqual(pets[i].PetName, result[i].PetName);
                Assert.AreEqual(pets[i].Gender, result[i].PetGender);
                Assert.AreEqual(pets[i].PetParentID, result[i].OwnerID);
                // Assert.AreEqual(pets[i].PetImage, result[i].petImage); // Asserting byte arrays might need a custom comparer
            }
        }

        [TestMethod]
        public async Task ConvertPetsToCardPetDetailsDto_Returns_Null_When_Pets_Is_Null()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository()); // Assuming no dependencies needed
            List<Pet> pets = null;

            // Act
            var result = await controller.ConvertPetsToCardPetDetailsDtoAsync(pets);

            // Assert
            Assert.IsNull(result); // Assert that the result is null
        }

        [TestMethod]
        public async Task ConvertPetsToCardPetDetailsDto_Returns_Empty_List_When_Pets_Is_Empty()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository()); // Assuming no dependencies needed
            List<Pet> pets = new List<Pet>(); // Empty list

            // Act
            var result = await controller.ConvertPetsToCardPetDetailsDtoAsync(pets);

            // Assert
            Assert.IsNotNull(result); // Assert that the result is not null
            Assert.AreEqual(0, result.Count); // Assert that the result is an empty list
        }

        [TestMethod]
        public async Task GetPetsByIDsInPetCardDto_Returns_BadRequest_When_Ids_Null()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository());

            // Act
            var result = await controller.GetPetsByIDsInPetCardDto(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("No IDs provided", ((BadRequestErrorMessageResult)result).Message);
        }

        [TestMethod]
        public async Task GetPetsByIDsInPetCardDto_Returns_BadRequest_When_Ids_Empty()
        {
            // Arrange
            var controller = new PetsController(new PetsRepository());

            // Act
            var result = await controller.GetPetsByIDsInPetCardDto(new int[0]);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("No IDs provided", ((BadRequestErrorMessageResult)result).Message);
        }

        [TestMethod]
        public async Task GetPetsByIDsInPetCardDto_Returns_BadRequest_When_Pets_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetPetsByIdsAsync(It.IsAny<int[]>())).ReturnsAsync((List<Pet>)null); // No pets found

            var controller = new PetsController(mockRepo.Object);

            // Act
            var result = await controller.GetPetsByIDsInPetCardDto(new[] { 1, 2, 3 });

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual("Pets not found for the given ids", ((BadRequestErrorMessageResult)result).Message);
        }


        [TestMethod]
        public async Task GetPetsByIDsInPetCardDto_Returns_Ok_With_CardPetDetailsDto_When_Pets_Found()
        {
            // Arrange
            var pets = new List<Pet>
            {
                new Pet { PetID = 1, PetName = "Fido", Gender = "Male", PetParentID = 1, PetImage = new byte[0] },
                new Pet { PetID = 2, PetName = "Fluffy", Gender = "Female", PetParentID = 2, PetImage = new byte[0] }
            };

            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetPetsByIdsAsync(It.IsAny<int[]>())).ReturnsAsync(pets);

            var controller = new PetsController(mockRepo.Object); // Use actual PetsController, not mock

            // Act
            var result = await controller.GetPetsByIDsInPetCardDto(new[] { 1, 2, 3 });

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<CardPetDetailsDto>>));

            // Verify that GetPetsByIdsAsync is called with the correct parameter
            mockRepo.Verify(repo => repo.GetPetsByIdsAsync(new[] { 1, 2, 3 }), Times.Once);
        }


        [TestMethod]
        public async Task FilterPets_Returns_Ok_With_FilteredPets_When_Pets_Found()
        {
            // Arrange
            var filterParams = new PetFilterParams
            {
                PetName = "Fido",
                Species = "Dog",
                PetIds = new int[] { 1, 2, 3 }
            };

            var filteredPets = new List<Pet>
    {
        new Pet { PetID = 1, PetName = "Fido", Species = "Dog", /* Add other properties as needed */ },
        new Pet { PetID = 2, PetName = "Fido", Species = "Dog", /* Add other properties as needed */ }
    };

            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.FilterPetsAsync(filterParams)).ReturnsAsync(filteredPets);

            var controller = new PetsController(mockRepo.Object);

            // Act
            var result = await controller.FilterPets(filterParams) as OkNegotiatedContentResult<List<Pet>>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(filteredPets.Count, result.Content.Count);
            // Add more assertions to check each property of the pets if needed
        }

        [TestMethod]
        public async Task FilterPets_Returns_NotFound_When_No_Pets_Found()
        {
            // Arrange
            var filterParams = new PetFilterParams
            {
                PetName = "NonExistentPet",
                Species = "NonExistentSpecies",
                PetIds = new int[] { 100, 200, 300 } // IDs that don't exist
            };

            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.FilterPetsAsync(filterParams)).ReturnsAsync(new List<Pet>());

            var controller = new PetsController(mockRepo.Object);

            // Act
            var result = await controller.FilterPets(filterParams);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetPetsByIds_Returns_Ok_With_Pets()
        {
            // Arrange
            int[] petIds = { 1, 2, 3 };
            var mockRepo = new Mock<IPetsRepository>();
            var expectedPets = new List<Pet>
            {
                new Pet { PetID = 1, PetName = "Fido", Gender = "Male", PetParentID = 1, PetImage = new byte[0] },
                new Pet { PetID = 2, PetName = "Fluffy", Gender = "Female", PetParentID = 2, PetImage = new byte[0] },
                new Pet { PetID = 3, PetName = "Max", Gender = "Male", PetParentID = 3, PetImage = new byte[0] }
            };
            mockRepo.Setup(repo => repo.GetPetsByIdsAsync(petIds)).ReturnsAsync(expectedPets);
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetPetsByIds(petIds);
            var contentResult = actionResult as OkNegotiatedContentResult<List<Pet>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedPets, contentResult.Content.ToList());

            // Verify that GetPetsByIdsAsync is called with the correct parameter
            mockRepo.Verify(repo => repo.GetPetsByIdsAsync(petIds), Times.Once);
        }

        [TestMethod]
        public async Task GetPetsByIds_Returns_BadRequest_When_No_Pet_Ids_Provided()
        {
            // Arrange
            int[] petIds = null;
            var mockRepo = new Mock<IPetsRepository>();
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetPetsByIds(petIds);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            BadRequestErrorMessageResult badRequestResult = actionResult as BadRequestErrorMessageResult;
            Assert.AreEqual("Please provide at least one pet ID.", badRequestResult.Message);

            // Verify that GetPetsByIdsAsync is not called
            mockRepo.Verify(repo => repo.GetPetsByIdsAsync(It.IsAny<int[]>()), Times.Never);
        }

        [TestMethod]
        public async Task GetPetsByIds_Returns_BadRequest_When_No_Pets_Found()
        {
            // Arrange
            int[] petIds = { 1, 2, 3 };
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetPetsByIdsAsync(petIds)).ReturnsAsync(new List<Pet>());
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetPetsByIds(petIds);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(BadRequestErrorMessageResult));
            BadRequestErrorMessageResult badRequestResult = actionResult as BadRequestErrorMessageResult;
            Assert.AreEqual("No pets found for the provided IDs.", badRequestResult.Message);

            // Verify that GetPetsByIdsAsync is called with the correct parameter
            mockRepo.Verify(repo => repo.GetPetsByIdsAsync(petIds), Times.Once);
        }

        [TestMethod]
        public async Task GetMorePets_Returns_Ok_With_Pets()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var mockRepo = new Mock<IPetsRepository>();
            var expectedPets = new List<Pet>
            {
                new Pet { PetID = 1, PetName = "Fido", Gender = "Male", PetParentID = 1, PetImage = new byte[0] },
                new Pet { PetID = 2, PetName = "Fluffy", Gender = "Female", PetParentID = 2, PetImage = new byte[0] }
            };
            mockRepo.Setup(repo => repo.GetPetsAsync(pageNumber, pageSize)).ReturnsAsync(expectedPets);
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetMorePets(pageNumber, pageSize);
            var contentResult = actionResult as OkNegotiatedContentResult<List<Pet>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedPets, contentResult.Content);

            // Verify that GetPetsAsync is called with the correct parameters
            mockRepo.Verify(repo => repo.GetPetsAsync(pageNumber, pageSize), Times.Once);
        }

        [TestMethod]
        public async Task GetMorePets_Returns_InternalServerError_When_Exception_Occurs()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 10;
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.GetPetsAsync(pageNumber, pageSize)).ThrowsAsync(new Exception("Test exception"));
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.GetMorePets(pageNumber, pageSize);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(ExceptionResult));

            // Verify that GetPetsAsync is called with the correct parameters
            mockRepo.Verify(repo => repo.GetPetsAsync(pageNumber, pageSize), Times.Once);
        }

        [TestMethod]
        public async Task FilterPetsWithPagination_Returns_Ok_With_Filtered_Pets()
        {
            // Arrange
            var filterParams = new PetFilterParams
            {
                PetName = "Fido",
                Species = "Dog",
                PetIds = new int[] { 1, 2 }
            };
            int pageNumber = 1;
            int pageSize = 10;
            var mockRepo = new Mock<IPetsRepository>();
            var expectedPets = new List<Pet>
            {
                new Pet { PetID = 1, PetName = "Fido", Age = "3", Gender = "Male", Species = "Dog", PetParentID = 1, PetImage = new byte[0] },
                new Pet { PetID = 2, PetName = "Fido", Age = "5", Gender = "Female", Species = "Dog", PetParentID = 2, PetImage = new byte[0] }
            };
            mockRepo.Setup(repo => repo.FilterPetsPerPageAsync(filterParams, pageNumber, pageSize)).ReturnsAsync(expectedPets);
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.FilterPetsWithPagination(filterParams, pageNumber, pageSize);
            var contentResult = actionResult as OkNegotiatedContentResult<List<Pet>>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            CollectionAssert.AreEqual(expectedPets, contentResult.Content);

            // Verify that FilterPetsPerPageAsync is called with the correct parameters
            mockRepo.Verify(repo => repo.FilterPetsPerPageAsync(filterParams, pageNumber, pageSize), Times.Once);
        }

        [TestMethod]
        public async Task FilterPetsWithPagination_Returns_NotFound_When_No_Pets_Found()
        {
            // Arrange
            var filterParams = new PetFilterParams
            {
                PetName = "Unknown",
                Species = "Unknown",
                PetIds = new int[2]
            };
            int pageNumber = 1;
            int pageSize = 10;
            var mockRepo = new Mock<IPetsRepository>();
            mockRepo.Setup(repo => repo.FilterPetsPerPageAsync(filterParams, pageNumber, pageSize)).ReturnsAsync(new List<Pet>());
            var controller = new PetsController(mockRepo.Object);

            // Act
            IHttpActionResult actionResult = await controller.FilterPetsWithPagination(filterParams, pageNumber, pageSize);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));

            // Verify that FilterPetsPerPageAsync is called with the correct parameters
            mockRepo.Verify(repo => repo.FilterPetsPerPageAsync(filterParams, pageNumber, pageSize), Times.Once);
        }
    }
}
