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
        public async Task GetAllPets_WithPets_ReturnOk()
        {
            var mockRepo = new Mock<PetsRepository>();
            mockRepo.Setup(repo => repo.GetAllPetsAsync()).ReturnsAsync(new List<Pet> { new Pet {PetID=1,PetParentID=2,Species="Dog",Breed="Pug",PetName="name_test",Age="2" } });

            var controller = new PetsController(mockRepo.Object);
            var result = await controller.GetAllPets();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<List<Pet>>));

            var contentResult = result as OkNegotiatedContentResult<List<Pet>>;
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Count);
            Assert.AreEqual("name_test", contentResult.Content.First().PetName);
            Assert.AreEqual(1,contentResult.Content.First().PetID);
            Assert.AreEqual(2, contentResult.Content.First().PetParentID);
            Assert.AreEqual("Dog", contentResult.Content.First().Species);
            Assert.AreEqual("Pug", contentResult.Content.First().Breed);
            Assert.AreEqual("2", contentResult.Content.First().Age);

        }
        [TestMethod]
        public async Task GetAllPets_WithoutPets_ReturnsBadRequest()
        {
            //Arrange
            var mockRepo = new Mock<PetsRepository>();
            mockRepo.Setup(repo => repo.GetAllPetsAsync()).ReturnsAsync(new List<Pet>());
            var controller = new PetsController(mockRepo.Object);

            // Act
            var result = await controller.GetAllPets();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }



    }
}
