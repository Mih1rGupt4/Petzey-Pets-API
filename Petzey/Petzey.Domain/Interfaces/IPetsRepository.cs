using Petzey.Domain.Entities;
using Petzey.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Interfaces
{
    public interface IPetsRepository
    {
        Task<List<Pet>> GetAllPetsAsync();
        Task<List<Pet>> searchPetsAsync(string searchTerm);
        Task<List<int>> FilterPetsAsync(PetFilterParams petFilterParams);
        Task<FilteredPetsDto> FilterPetsAsync(PetFilterParams petFilterParams, int pageNumber, int pageSize);
        Task<List<Pet>> FilterPetsPerPageAsync(PetFilterParams petFilterParams, int pageNumber, int pageSize);

        Task<List<Pet>> FilterPetsAndIdAsync(PetFilterParams petFilterParams, int[] petIds);
        Task<List<Pet>> GetPetsByIdsAsync(int[] petIds);

        Task<Pet> AddPet(Pet pet);
        Task<Pet> EditPet(Pet pet);
        Task<List<Pet>> GetPetsByIDs(int[] ids);

        Task<Pet> GetPetDetailsByPetIDAsync(int id);
        Task<List<Pet>> GetPetsAsync(int pageNumber, int pageSize);
        Task<List<Pet>> GetPetsByPetParentIdAsync(string parentId);
        Task<bool> DeletePetAsync(int petId);
        Task<bool> AddLastAppointmentDate(DateTime date, int id);

        Task<int> FilterPetsCount(PetFilterParams petFilterParams);
        Task<List<Allergy>> Allergies();
        Task<List<PetAllergies>> GetAllPetAllergies(int id);
        Task<int> AddPetAllergy(List<int> allergyIDs, int petID);
        Task<bool> DeletePetAllergy(int petID);
    }
}
