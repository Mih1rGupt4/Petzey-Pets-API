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
        public PetsController() : this(new PetsRepository())
        {

        }

        public PetsController(IPetsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
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
                return BadRequest();
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
                return BadRequest("No pets found for the provided IDs.");
            }
        }

        [HttpGet]
        [Route("details/{id}")]
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
        public async Task<IHttpActionResult> AddPet([FromBody] Pet pet)
        {
            var newPet = await _repo.AddPet(pet);
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
        public async Task<IHttpActionResult> EditPet([FromBody] Pet pet)
        {
            var editedPet = await _repo.EditPet(pet);
            if (editedPet != null)
            {
                return Ok("Edit successful");
            }
            return BadRequest("Edit unsuccessful");
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
        [HttpPut]
        [Route("AddLastAppointmentDate/{id}")]
        public async Task<IHttpActionResult> addLastAppointmentDate(int id, [FromBody] DateTime date)
        {
            bool status = await  _repo.AddLastAppointmentDate(date, id);
            if (status)
            {
                return Ok("Added appointment date to pet");
            }
            return BadRequest("Failed in adding appointment date to pet");
        }

        public IHttpActionResult OkOrNotFound(object obj)
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
