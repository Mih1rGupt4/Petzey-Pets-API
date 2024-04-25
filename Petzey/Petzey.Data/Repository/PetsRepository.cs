using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {
        public PetzeyPetsDbContext db = new PetzeyPetsDbContext();

        public async Task<List<Pet>> GetAllPetsAsync()
        {
            return await db.Pets.ToListAsync();
        }

        public async Task<List<Pet>> searchPetsAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var Allpets = await db.Pets.ToListAsync(); // Return all pets if searchTerm is empty
                return (Allpets);
            }
            else
            {

                var filteredPets = db.Pets.Where(pet =>
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
    }
}
