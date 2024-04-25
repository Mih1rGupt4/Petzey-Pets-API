using Petzey.Data;
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
        public PetzeyPetsDbContext db = new PetzeyPetsDbContext();

        [HttpGet]
        public IHttpActionResult GetAllPets()
        {
            var pets = db.Pets.ToList();
   
            if (pets.Any())
                return Ok(pets);
            else
                return Ok("No pets found ");
        }

        [HttpGet]
        [Route("searchPets")]
        public IHttpActionResult SearchPets([FromBody] string searchTerm)
        {
            var searchResults = db.Pets.Where(pet =>
            pet.PetName.ToLower().Contains(searchTerm.ToLower()) ||
            pet.Species.ToLower().Contains(searchTerm.ToLower()) ||
            pet.Breed.ToLower().Contains(searchTerm.ToLower()) 
            // Add
            ).ToList();

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
