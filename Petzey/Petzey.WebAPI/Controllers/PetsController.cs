using Petzey.Data;
using Petzey.Data.Repository;
using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Petzey.WebAPI.Controllers
{
    [RoutePrefix("api/pets")]
    public class PetsController : ApiController
    {
       
        public IPetsRepository repo = new PetsRepository();

        [HttpGet]
        public async Task<IHttpActionResult> GetAllPets()
        {
            List<Pet> pets = await repo.GetAllPetsAsync(); // Call the async method

            if (pets.Any())
            {
                return Ok(pets);
            }
            else
            {
                return Ok("No pets found ");
            }
        }

       

        [HttpGet]
        [Route("searchPets")]
        public async Task<IHttpActionResult> SearchPets( string searchTerm)
        {
            var searchResults = await repo.searchPetsAsync(searchTerm);
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
