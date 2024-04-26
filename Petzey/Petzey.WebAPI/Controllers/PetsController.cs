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

        public IPetsRepository _repo = new PetsRepository();

        [HttpGet]
        public async Task<IHttpActionResult> GetAllPets()
        {
            List<Pet> pets = await _repo.GetAllPetsAsync(); // Call the async method

            if (pets.Any())
            {
                return Ok(pets);
            }
            else
            {
                return Ok("No pets found ");
            }
        }

        //[HttpGet]
        //[Route("searchPets")]
        //public async Task<IHttpActionResult> SearchPets(string searchTerm)
        //{
        //    var searchResults = await _repo.searchPetsAsync(searchTerm);
        //    if (searchResults.Any())
        //    {
        //        return Ok(searchResults);
        //    }
        //    else
        //    {
        //        return Ok("No pets found matching the search criteria.");
        //    }
        //}

        [HttpPost]
        [Route("filter")]
        public async Task<IHttpActionResult> FilterPets([FromBody] PetFilterParams filterParams)
        {
            var pets = await _repo.FilterPetsAsync(filterParams);
            if (pets.Any())
            {
                return Ok(pets);
            }
            else
            {
                return Ok("No pets found matching the search criteria.");
            }
        }

        //[HttpPost]
        //[Route("filterids")]
        //public async Task<IHttpActionResult> FilterPetsAndIds([FromBody] PetFilterParams filterParams,[FromUri]int[] petIds)
        //{
        //    var pets = await _repo.FilterPetsAndIdAsync(filterParams,petIds);
        //    if (pets.Any())
        //    {
        //        return Ok(pets);
        //    }
        //    else
        //    {
        //        return Ok("No pets found matching the search criteria.");
        //    }
        //}

        [HttpPost]
        [Route("Ids")]
        public async Task<IHttpActionResult> GetPetsByIds([FromBody]int[] petIds)
        {
            if (petIds == null || !petIds.Any())
            {
                return BadRequest("Please provide at least one pet ID.");
            }

            var pets = await _repo.GetPetsByIdsAsync(petIds);

            if (pets.Any())
            {
                return Ok(pets);
            }
            else
            {
                return Ok("No pets found for the provided IDs.");
            }
        }


    }
}
