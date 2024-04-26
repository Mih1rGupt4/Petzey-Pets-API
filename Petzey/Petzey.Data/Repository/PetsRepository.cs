using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {
        PetzeyPetsDbContext db = new PetzeyPetsDbContext();

        public async Task<bool> DeletePetAsync(int petId)
        {
            Pet pet = await db.Pets.FindAsync(petId);
            if (pet == null)
                return false;

            db.Pets.Remove(pet);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Pet>> GetPetsByPetParentIdAsync(int parentId)
        {
            return await db.Pets.Where(p => p.PetParentID == parentId).ToListAsync();
        }
    }
}
