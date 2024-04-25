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
        PetzeyPetsDbContext db = new PetzeyPetsDbContext();

        public bool DeletePet(int petId)
        {
            Pet pet = db.Pets.Find(petId);
            if (pet == null)
                return false;

            db.Pets.Remove(pet);
            db.SaveChanges();
            return true;
        }

        public List<Pet> GetPetsByPetParentId(int parentId)
        {
            return db.Pets.Where(p => p.PetParentID == parentId).ToList();
        }
    }
}
