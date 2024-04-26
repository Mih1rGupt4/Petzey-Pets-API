using Petzey.Data.Repository;
using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Management;

namespace Petzey.WebAPI.Controllers
{
    public class PetsController : ApiController
    {
        public PetsRepository _repo = new PetsRepository();
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
    }
}
