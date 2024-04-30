using Petzey.Domain.Entities;
using Petzey.Domain.Dtos;
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
            return filteredPets.ToList();
        }

        //public async Task<List<Pet>> FilterPetsPerPageAsync(PetFilterParams petFilterParams, int pageNumber, int pageSize)
        //{
        //    var filteredPets = await _db.Pets.Where(p => (p.PetName.Contains(petFilterParams.PetName) || petFilterParams.PetName == null)
        //    && (p.Species.Contains(petFilterParams.Species) || petFilterParams.Species == null)
        //    && (petFilterParams.PetIds.Contains(p.PetID) || !petFilterParams.PetIds.Any())).ToListAsync();

        //    return filteredPets.OrderBy(p => p.PetID)
        //        .Skip((pageNumber - 1) * pageSize)
        //        .Take(pageSize)
        //        .ToList();
        //}

        public async Task<List<Pet>> FilterPetsPerPageAsync(PetFilterParams petFilterParams, int pageNumber, int pageSize)
        {
            IQueryable<Pet> query = _db.Pets;

            // Filter by pet name
            if (!string.IsNullOrEmpty(petFilterParams.PetName))
                query = query.Where(p => p.PetName.Contains(petFilterParams.PetName));

            // Filter by species
            if (!string.IsNullOrEmpty(petFilterParams.Species))
                query = query.Where(p => p.Species.Contains(petFilterParams.Species));

            // Filter by pet IDs
            if (petFilterParams.PetIds != null && petFilterParams.PetIds.Any())
                query = query.Where(p => petFilterParams.PetIds.Contains(p.PetID));
            

            // Paginate and materialize
            List<Pet> filteredPets = await query.OrderBy(p => p.PetID)
                                                 .Skip((pageNumber - 1) * pageSize)
                                                 .Take(pageSize)
                                                 .ToListAsync();

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

            // Perform the query
            var pets = await _db.Pets
                .Where(pet => petIds.Contains(pet.PetID))
                .ToListAsync();

            // If no pets are found, return null
            if (pets.Count == 0)
                return null;

            return pets;
        }

        public async Task<Pet> AddPet(Pet pet)
        {
            _db.Pets.Add(pet);
            await _db.SaveChangesAsync();
            return pet;
        }

        public async Task<Pet> EditPet(Pet pet)
        {
            // Check if a pet with the same ID exists in the database
            var existingPet = await _db.Pets.FindAsync(pet.PetID);

            if (existingPet != null)
            {
                // Update the existing pet with the new values
                _db.Entry(existingPet).CurrentValues.SetValues(pet);
                await _db.SaveChangesAsync();
                return existingPet;
            }

            // Return null if the pet with the given ID doesn't exist
            return null;
        }

        public async Task<List<Pet>> GetPetsByIDs(int[] ids)
        {
            List<Pet> petsByID = new List<Pet>();
            foreach (int id in ids)
            {
                Pet pet = await _db.Pets.FirstOrDefaultAsync(p => p.PetID == id);
                if (pet != null)
                {
                    petsByID.Add(pet);
                }
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

        public async Task<int> FilterPetsCount(PetFilterParams petFilterParams)
        {
            IQueryable<Pet> query = _db.Pets;

            // Filter by pet name
            if (!string.IsNullOrEmpty(petFilterParams.PetName))
                query = query.Where(p => p.PetName.Contains(petFilterParams.PetName));

            // Filter by species
            if (!string.IsNullOrEmpty(petFilterParams.Species))
                query = query.Where(p => p.Species.Contains(petFilterParams.Species));

            // Filter by pet IDs
            if (petFilterParams.PetIds != null && petFilterParams.PetIds.Any())
                query = query.Where(p => petFilterParams.PetIds.Contains(p.PetID));

            return await query.CountAsync(); // Use CountAsync to asynchronously count the number of results
        }

    }
}
