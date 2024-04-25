using Petzey.Data;
using Petzey.Data.Repository;
using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Petzey.WebAPI.Controllers
{
    [RoutePrefix("api/pets")]
    public class PetsController : ApiController
    {
       
        public IPetsRepository repo = new PetsRepository();

        [HttpGet]
        public IHttpActionResult GetAllPets()
        {
            List<Pet> pets = repo.getAllPets();

            if (pets.Any())
                return Ok(pets);
            else
                return Ok("No pets found ");
        }

       

        [HttpGet]
        [Route("searchPets")]
        public IHttpActionResult SearchPets( string searchTerm)
        {
            var searchResults = repo.searchPets(searchTerm);
            if (searchResults.Any())
            {
                return Ok(searchResults);
            }
            else
            {
                return Ok("No pets found matching the search criteria.");
            }
        }

       
    }
}
