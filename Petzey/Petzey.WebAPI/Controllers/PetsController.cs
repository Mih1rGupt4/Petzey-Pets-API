using Petzey.Data.Repository;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Petzey.WebAPI.Controllers
{
    [RoutePrefix("api/Pets")]
    public class PetsController : ApiController
    {
        private readonly IPetsRepository _repo;

        public PetsController()
        {
            _repo = new PetsRepository();
        }


        [HttpGet]
        [Route("parentid/{parentId}")]
        public async Task<IHttpActionResult> GetPetsByPetParentId(int parentId)
        {
            var pets = await _repo.GetPetsByPetParentIdAsync(parentId);
            
            if (pets.Any())
            {
                return Ok(pets);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> DeletePet(int id)
        {
            bool success = await _repo.DeletePetAsync(id);

            if (!success)
                return NotFound();

            return Ok($"Pet with ID {id} successfully deleted");
        }
    }
}
