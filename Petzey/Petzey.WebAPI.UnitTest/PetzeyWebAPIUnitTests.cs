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

namespace Petzey.WebAPI.UnitTest
{
    [TestClass]
    public class PetzeyWebAPIUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {

        }

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

    }
}
