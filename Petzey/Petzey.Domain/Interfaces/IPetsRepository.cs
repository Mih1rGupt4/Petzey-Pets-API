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
        bool DeletePet(int petId);
        List<Pet> GetPetsByPetParentId(int parentId);
    }
}
