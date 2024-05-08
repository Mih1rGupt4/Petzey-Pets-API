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
        public async Task<IHttpActionResult> FilterPets([FromBody] PetFilterParams filterParams) //Method to get pets with a search criteria
        {
            var filteredPets = await _repo.FilterPetsAsync(filterParams); // call the async method

            // if found any pets, then return 
            if (filteredPets.Any())
            {
                return Ok(filteredPets);  // Return the Pets after Filtering based on filterParams
            }
            else
            {
                return NotFound(); // Return 404 Not Found when no pets are found
            }
        }

        [HttpPost]
        [Route("filters", Name = "FilterPetsWithPagination")]
        public async Task<IHttpActionResult> FilterPetsWithPagination([FromBody] PetFilterParams filterParams, [FromUri] int pageNumber, [FromUri] int? pageSize = 10) // get pets with search criteria for pagination
        {
            var filteredPetsPerPage = await _repo.FilterPetsPerPageAsync(filterParams, pageNumber, (int)pageSize); // call the async method

            // if found any pets, then return 
            if (filteredPetsPerPage.Any())
            {
                return Ok(filteredPetsPerPage);  // Return the Pets after Filtering based on filterParams and Pagination
            }
            else
            {
                return NotFound(); // Return 404 Not Found when no pets are found
            }
        }

        [HttpPost]
        [Route("filters/count")]
        public async Task<IHttpActionResult> FilterPetsCount([FromBody] PetFilterParams filterParams)// get the number of pets after applying the search criteria
        {
            var filteredPetsCount = await _repo.FilterPetsCount(filterParams);// call the async method

            if (filteredPetsCount > 0) // check if there are any pets meeting the criteria
            {
                return Ok(filteredPetsCount); // if greater than zero return the number with an OK request
            }
            else
            {
                return NotFound(); // Return 404 Not Found when no pets are found
            }
        }

       

        [HttpPost]
        [Route("Ids")]
        public async Task<IHttpActionResult> GetPetsByIds([FromBody]int[] petIds)// get the pets based on the IDs provided
        {
            // If ids is null or empty, return BadRequest immediately
            if (petIds == null || !petIds.Any())
            {
                return BadRequest("Please provide at least one pet ID.");// check if the given array of IDs is empty or not, if empty return a bad request
            }

            var pets = await _repo.GetPetsByIdsAsync(petIds);  // Get all pets based on array of petIds 

            if (pets.Any())
            {
                return Ok(pets);// return the pets data if there are pets with that ids
            }
            else
            {
                return BadRequest("No pets found for the provided IDs.");// else return a bad request
            }
        }

        [HttpGet]
        [Route("details/{id}")]
        public async Task<IHttpActionResult> GetPetDetailsByPetID(int id)//get pet data for a particular pet id
        {
            Pet pet = await _repo.GetPetDetailsByPetIDAsync(id); // call the function
            return OkOrNotFound(pet);//return pet with ok or not found based on the avilability
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetMorePets(int pageNumber, int pageSize = 10) // Default pageSize is 10
        {
            try
            {
                List<Pet> pets = await _repo.GetPetsAsync(pageNumber, pageSize);//getting pets based on the pet size
                return Ok(pets);// returning the pets 
            }
            catch (Exception ex)
            {
                // Log the exception if any exception is caught 
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("addnewpet")]
        public async Task<IHttpActionResult> AddPet([FromBody] Pet pet) //add a new pet
        {
            var newPet = await _repo.AddPet(pet);//call function to add a pet
            if(newPet != null)
            {
                return Ok("New Pet added");// if the pet was added return an Ok request
            }
            return BadRequest("Pet not added!");// else return a BadRequest
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
        public async Task<IHttpActionResult> EditPet([FromBody] Pet pet)// to edit the details of a particular pet
        {
            var editedPet = await _repo.EditPet(pet);// call the function to edit pet
            //checks if the returned value is null to verify if edit is successful or not
            if (editedPet != null)
            {
                return Ok("Edit successful");// OK if successful
            }
            return BadRequest("Edit unsuccessful");//BadRequest if unsuccessful
        }


        [HttpGet]
        [Route("parentid/{parentId}")]
        public async Task<IHttpActionResult> GetPetsByPetParentId(string parentId)//to get Pet by PetParent ID
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
        public async Task<IHttpActionResult> DeletePet(int id)// to delete a pet
        {
            bool success = await _repo.DeletePetAsync(id);// calls the function to delete a pet and returns a bool

            if (!success)//if returned value is false then delete failed
                return NotFound();
            // if the pet is not found then it will return a status code of 404

            return Ok($"Pet with ID {id} successfully deleted");//if returned value is true then edit successful
        }
        [HttpPut]
        [Route("AddLastAppointmentDate/{id}")]
        public async Task<IHttpActionResult> addLastAppointmentDate(int id, [FromBody] DateTime date)//to update the last appointment date of the pet
        {
            bool status = await  _repo.AddLastAppointmentDate(date, id);//call the function to update and wait for the status
            if (status)
            {
                return Ok("Added appointment date to pet");//Ok if date is updated
            }
            return BadRequest("Failed in adding appointment date to pet");// Bad Request if not updated / error in input data
        }

        public IHttpActionResult OkOrNotFound(object obj)// function to verify if the object is found or not found
        {
            if(obj == null)
                return NotFound();
            else
                return Ok(obj);
        }

        public async Task<List<CardPetDetailsDto>> ConvertPetsToCardPetDetailsDtoAsync(List<Pet> pets)// function to convert the pet entity to pet dto
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
