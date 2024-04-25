using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {
        public PetzeyPetsDbContext db = new PetzeyPetsDbContext();

        public List<Pet> getAllPets()
        {
            return db.Pets.ToList();
        }

        public List<Pet> searchPets(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                var Allpets = db.Pets.ToList(); // Return all pets if searchTerm is empty
                return (Allpets);
            }
            else
            {

                var searchResults = db.Pets.Where(pet =>
                    pet.PetName.ToLower().Contains(searchTerm.ToLower()) ||
                    pet.Species.ToLower().Contains(searchTerm.ToLower()) ||
                    pet.Breed.ToLower().Contains(searchTerm.ToLower())
                    // Add
                    ).ToList();

               return searchResults;
            }
        }
    }
}
