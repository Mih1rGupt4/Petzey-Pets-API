using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {
        PetzeyPetsDbContext db;
        public PetsRepository()
        {
            db = new PetzeyPetsDbContext();
        }

        public Pet GetPetDetailsByPetID(int id)
        {
            return db.Pets.FirstOrDefault(p => p.PetID == id);
        }

        public List<Pet> GetMorePets(int pageNumber, int pageSize)
        {
            // Add OrderBy before Skip
            return db.Pets.OrderBy(p => p.PetID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            //return db.Pets.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}
