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
    public class PetsController : ApiController
    {
        IPetsRepository repo = new PetsRepository();

        [HttpGet]
        public IHttpActionResult GetPetDetailsByPetID(int id)
        {
            Pet pet = repo.GetPetDetailsByPetID(id);
            return OkOrNotFound(pet);
        }

        [HttpPost]
        public IHttpActionResult GetMorePets(int pageNumber, int pageSize = 10) // Default pageSize is 10
        {
            List<Pet> pets = repo.GetMorePets(pageNumber, pageSize);
            return Ok(pets);
        }


        private IHttpActionResult OkOrNotFound(object pet)
        {
            if(pet == null)
                return NotFound();
            else
                return Ok(pet);
        }
    }
}
