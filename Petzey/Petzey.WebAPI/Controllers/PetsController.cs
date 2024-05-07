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
using System.Web.Http.Cors;
using System.Web.Management;

namespace Petzey.WebAPI.Controllers
{
    [RoutePrefix("api/pets")]
    [EnableCors("*","*","*")]
    public class PetsController : ApiController
    {
    
        IPetsRepository _repo;
        //public PetsController() : this(new PetsRepository())
        //{

        //}

        public PetsController(IPetsRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllPets() //Method to Get all pets
        {
            List<Pet> pets = await _repo.GetAllPetsAsync(); // Call the async method

            if (pets.Any())//checking is the returned list is empty or not
            {
                return Ok(pets);//return OK with list if not empty
            }
            else
            {
                return BadRequest();//return BadRequest if empty
            }
        }

        [HttpPost]
        [Route("filter")]
        public async Task<IHttpActionResult> FilterPets([FromBody] PetFilterParams filterParams) //Method to get 
        {
            var filteredPets = await _repo.FilterPetsAsync(filterParams);

            if (filteredPets.Any())
            {
                return Ok(filteredPets);
            }
            else
            {
                return NotFound(); // Return 404 Not Found when no pets are found
            }
        }

        [HttpPost]
        [Route("filters", Name = "FilterPetsWithPagination")]
        public async Task<IHttpActionResult> FilterPetsWithPagination([FromBody] PetFilterParams filterParams, [FromUri] int pageNumber, [FromUri] int? pageSize = 10)
        {
            var filteredPetsPerPage = await _repo.FilterPetsPerPageAsync(filterParams, pageNumber, (int)pageSize);

            if (filteredPetsPerPage.Any())
            {
                return Ok(filteredPetsPerPage);
            }
            else
            {
                return NotFound(); // Return 404 Not Found when no pets are found
            }
        }

        [HttpPost]
        [Route("filters/count")]
        public async Task<IHttpActionResult> FilterPetsCount([FromBody] PetFilterParams filterParams)
        {
            var filteredPetsCount = await _repo.FilterPetsCount(filterParams);

            if (filteredPetsCount > 0)
            {
                return Ok(filteredPetsCount);
            }
            else
            {
                return NotFound(); // Return 404 Not Found when no pets are found
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
        public async Task<IHttpActionResult> GetPetsByIDsInPetCardDto([FromBody] int[] ids)
        {
            // If ids is null or empty, return BadRequest immediately
            if (ids == null || ids.Length == 0)
            {
                return BadRequest("No IDs provided");
            }

            var petsByIDs = await _repo.GetPetsByIdsAsync(ids);

            // If petsByIDs is null, it indicates no pets were found for the given IDs
            if (petsByIDs == null)
            {
                return BadRequest("Pets not found for the given ids");
            }

            var petCardDtos = await ConvertPetsToCardPetDetailsDtoAsync(petsByIDs);

            return Ok(petCardDtos);
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
        public async Task<IHttpActionResult> GetPetsByPetParentId(string parentId)
        {
            var pets = await _repo.GetPetsByPetParentIdAsync(parentId);
            if (pets.Any())
            {
                // If pets any pets are found it will return the pets along with the status code of 200
                return Ok(pets);   
            }
            else
            {
                // If there are no pets with that particular ParentID then it will return 404
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
            // if the pet is not found then it will return a status code of 404

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

        public async Task<List<CardPetDetailsDto>> ConvertPetsToCardPetDetailsDtoAsync(List<Pet> pets)
        {
            if (pets == null)
            {
                return null; // Return null if pets is null
            }

            // No asynchronous operations here, but marked as async to indicate it can be used in async contexts
            return await Task.FromResult(pets.Select(pet => new CardPetDetailsDto
            {
                PetID = pet.PetID,
                PetName = pet.PetName,
                PetGender = pet.Gender,
                OwnerID = pet.PetParentID,
                petImage = pet.PetImage
            }).ToList());
        }

    }
}
