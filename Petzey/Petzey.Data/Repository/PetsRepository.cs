using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Data.Repository
{
    public class PetsRepository : IPetsRepository
    {  
        PetzeyPetsDbContext _db = new PetzeyPetsDbContext();
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
    }
}
