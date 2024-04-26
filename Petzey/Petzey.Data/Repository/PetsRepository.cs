using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {
        public PetzeyPetsDbContext _db = new PetzeyPetsDbContext();

        public async Task<List<Pet>> GetAllPetsAsync()
        {
            return await _db.Pets.ToListAsync();
        }

        public async Task<List<Pet>> searchPetsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var Allpets = await _db.Pets.ToListAsync(); // Return all pets if searchTerm is empty
                return (Allpets);
            }
            else
            {

                var filteredPets = _db.Pets.Where(pet =>
                     pet.PetName.ToLower().Contains(searchTerm.ToLower()) ||
                     pet.Species.ToLower().Contains(searchTerm.ToLower()) ||
                     pet.Breed.ToLower().Contains(searchTerm.ToLower())
                 // Add more criteria
                 );

                // Appling ToListAsync after filtering for complex scenarios
                var searchResults = await filteredPets.ToListAsync();
                return searchResults;
            }
        }

        public async Task<List<Pet>> FilterPetsAsync(PetFilterParams petFilterParams)
        {

          // var pets =  db.Pets.Where(pet => petFilterParams.PetIds.Contains(pet.PetID));

            var filteredPets =await _db.Pets.Where(p => (p.PetName.Contains(petFilterParams.PetName) || petFilterParams.PetName == null)
            && (p.Species.Contains(petFilterParams.Species) || petFilterParams.Species == null)
            && (petFilterParams.PetIds.Contains(p.PetID) || !petFilterParams.PetIds.Any())).ToListAsync();
            return filteredPets;
        }

        public async Task<List<Pet>> FilterPetsAndIdAsync(PetFilterParams petFilterParams,int[] petIds)
        {
            var filteredPets = await _db.Pets.Where(p => (p.PetName.Contains(petFilterParams.PetName) || petFilterParams.PetName == null)
            && (p.Species.Contains(petFilterParams.Species) || petFilterParams.Species == null) && (petIds.Contains(p.PetID))).ToListAsync();
            return filteredPets;
        }


        public async Task<List<Pet>> GetPetsByIdsAsync(int[] petIds)
        {
            return await _db.Pets.Where(pet => petIds.Contains(pet.PetID)).ToListAsync();
        }
    }
}
