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
        Task<bool> DeletePetAsync(int petId);
        Task<List<Pet>> GetPetsByPetParentIdAsync(int parentId);
    }
}
