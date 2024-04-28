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




    }
}
