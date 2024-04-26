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
using System.Web.Management;

namespace Petzey.WebAPI.Controllers
{
    public class PetsController : ApiController
    {
    
        IPetsRepository _repo;
        public PetsController()
        {
            _repo = new PetsRepository();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetPetDetailsByPetID(int id)
        {
            Pet pet = await _repo.GetPetDetailsByPetIDAsync(id);
            return OkOrNotFound(pet);
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetMorePets(int pageNumber, int pageSize = 10) // Default pageSize is 10
        {
            try
            {
                List<Pet> pets = await _repo.GetPetsAsync(pageNumber, pageSize);
                return Ok(pets);
            }
            catch (Exception ex)
            {
                // Log the exception
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult AddPet([FromBody] Pet pet)
        {
            var newPet = _repo.AddPet(pet);
            if(newPet != null)
            {
                return Ok("New Pet added");
            }
            return BadRequest("Pet not added!");
        }
        [HttpPost]
        [Route("api/getPetsByIDs")]
        public IHttpActionResult GetPetsByIDs([FromBody] int[] ids)
        {
            var petsByIDs = _repo.GetPetsByIDs(ids);
            if(petsByIDs != null)
            {
                return Ok(petsByIDs);
            }
            return BadRequest("Pets not found for the given ids");
        }

        [HttpPut]
        public IHttpActionResult EditPet([FromBody] Pet pet)
        {
            var editPet = _repo.EditPet(pet);
            if(editPet != null)
            {
                return Ok("edit successfull");
            }
            return BadRequest("edit unsuccessfull");
        }

        private IHttpActionResult OkOrNotFound(object obj)
        {
            if(obj == null)
                return NotFound();
            else
                return Ok(obj);
        }
    }
}
