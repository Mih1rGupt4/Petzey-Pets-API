using Petzey.Data.Repository;
using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Petzey.WebAPI.Controllers
{
    public class PetsController : ApiController
    {
        public PetsRepository repo = new PetsRepository();
        [HttpPost]
        public IHttpActionResult AddPet([FromBody] Pet pet)
        {
            var newPet = repo.AddPet(pet);
            if(newPet != null)
            {
                return Ok("New Pet added");
            }
            return BadRequest("Pet not added!");
        }

        [HttpPut]
        public IHttpActionResult EditPet([FromBody] Pet pet)
        {
            var editPet = repo.EditPet(pet);
            if(editPet != null)
            {
                return Ok("edit successfull");
            }
            return BadRequest("edit unsuccessfull");
        }
    }
}
