using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {  
        private readonly PetzeyPetsDbContext _db;

        public PetsRepository(PetzeyPetsDbContext db)
        {
            _db = (db ?? throw new ArgumentNullException(nameof(db)));
        }

        // Default constructor creates a new PetzeyPetsDbContext instance
        public PetsRepository() : this(new PetzeyPetsDbContext())
        {
        }
        
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
        
        public async Task<Pet> AddPet(Pet pet)
        {
            _db.Pets.Add(pet);
            await _db.SaveChangesAsync();
            return pet;
        }

        public async Task<Pet> EditPet(Pet pet)
        {
            _db.Entry(pet).State = System.Data.Entity.EntityState.Modified;
            await _db.SaveChangesAsync();
            return pet;
        }
        
        public List<Pet> GetPetsByIDs(int[] ids)
        {
            List<Pet> petsByID = new List<Pet>();
            foreach (int id in ids)
            {
                petsByID.Add(_db.Pets.Find(id));
            }
            return petsByID;
        }

        public async Task<Pet> GetPetDetailsByPetIDAsync(int id)
        {
            return await _db.Pets.FirstOrDefaultAsync(p => p.PetID == id);
        }

        public async Task<List<Pet>> GetPetsAsync(int pageNumber, int pageSize)
        {
            return await _db.Pets.OrderBy(p => p.PetID)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync();
        }
        public async Task<bool> DeletePetAsync(int petId)
        {
            Pet pet = await _db.Pets.FindAsync(petId);
            if (pet == null)
                return false;

            _db.Pets.Remove(pet);
            await _db.SaveChangesAsync();
            return true;
        }
        
        public async Task<bool> AddLastAppointmentDate(DateTime date, int id)
        {
            var pet = _db.Pets.Find(id);
            pet.LastAppointmentDate = date;
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<List<Pet>> GetPetsByPetParentIdAsync(int parentId)
        {
            return await _db.Pets.Where(p => p.PetParentID == parentId).ToListAsync();
        }
    }
}
