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
        private readonly IPetzeyPetsDbContext _db;

        public PetsRepository(IPetzeyPetsDbContext db)
        {
            _db = (db ?? throw new ArgumentNullException(nameof(db)));
        }

        // Default constructor creates a new PetzeyPetsDbContext instance
        public PetsRepository() : this(new PetzeyPetsDbContext())
        {
        }
        
        public Pet AddPet(Pet pet)
        {
            _db.Pets.Add(pet);
            _db.SaveChanges();
            return pet;
        }

        public Pet EditPet(Pet pet)
        {
            _db.Entry(pet).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
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
    }
}
