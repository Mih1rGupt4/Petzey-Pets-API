using Petzey.Data.Repository;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Petzey.WebAPI.Controllers
{
    [RoutePrefix("api/Pets")]
    public class PetsController : ApiController
    {
        private readonly IPetsRepository _repo = new PetsRepository();

        [HttpGet]
        [Route("parentid/{parentId}")]
        public IHttpActionResult GetPetsByPetParentId(int parentId)
        {
            var pets = _repo.GetPetsByPetParentId(parentId);
            
            if (pets == null || pets.Count == 0)
            {
                return NotFound();
            }
            else
                return Ok(pets);
        }

        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeletePet(int id)
        {
            bool success = _repo.DeletePet(id);

            if (!success)
                return NotFound();

            return Ok($"Pet with ID {id} successfully deleted");
        }
    }
}
