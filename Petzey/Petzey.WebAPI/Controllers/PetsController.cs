using Petzey.Data;
using Petzey.Data.Repository;
using Petzey.Domain.Dtos;
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
    [RoutePrefix("api/pets")]
    public class PetsController : ApiController
    {
    
        IPetsRepository _repo;
        public PetsController()
        {
            _repo = new PetsRepository();
        }
        
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
        [Route("addnewpet")]
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
        [Route("getPetsByIDs")]
        public IHttpActionResult GetPetsByIDsInPetCardDto([FromBody] int[] ids)
        {
            var petsByIDs = _repo.GetPetsByIDs(ids);
            var petCardDtos = ConvertPetsToCardPetDetailsDto(petsByIDs);
            if(petCardDtos != null)
            {
                return Ok(petCardDtos);
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
                return Ok("No pets found ");
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
        [HttpPut]
        [Route("AddLastAppointmentDate/{id}")]
        public IHttpActionResult addLastAppointmentDate(int id, [FromBody] DateTime date)
        {
            var status = _repo.AddLastAppointmentDate(date, id);
            if (status.IsCompleted)
            {
                return Ok("Added appointment date to pet");
            }
            return BadRequest("Failed in adding appointment date to pet");
        }

        private IHttpActionResult OkOrNotFound(object obj)
        {
            if(obj == null)
                return NotFound();
            else
                return Ok(obj);
        }
        private List<CardPetDetailsDto> ConvertPetsToCardPetDetailsDto(List<Pet> pets)
        {
            List<CardPetDetailsDto> cardPetDetailsDto = new List<CardPetDetailsDto>();
            foreach (Pet pet in pets)
            {
                cardPetDetailsDto.Add(new CardPetDetailsDto
                {
                    PetID = pet.PetID,
                    PetName = pet.PetName,
                    PetAge = pet.Age,
                    PetGender = pet.Gender,
                    OwnerID = pet.PetParentID,
                    petImage = pet.PetImage
                });
            }
            return cardPetDetailsDto;
        }
    }
}
