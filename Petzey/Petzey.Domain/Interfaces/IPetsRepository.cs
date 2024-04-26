using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Interfaces
{
    public interface IPetsRepository
    {

        Pet AddPet(Pet pet);
        Pet EditPet(Pet pet);
        List<Pet> GetPetsByIDs(int[] ids);

        Task<Pet> GetPetDetailsByPetIDAsync(int id);
        Task<List<Pet>> GetPetsAsync(int pageNumber, int pageSize);

    }
}
