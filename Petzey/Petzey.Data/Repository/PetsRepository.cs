using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {  
        PetzeyPetsDbContext db = new PetzeyPetsDbContext();
        public Pet AddPet(Pet pet)
        {
            db.Pets.Add(pet);
            db.SaveChanges();
            return pet;
        }

        public Pet EditPet(Pet pet)
        {
            db.Entry(pet).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return pet;
        }
    }
}
