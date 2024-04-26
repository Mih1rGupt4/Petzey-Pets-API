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
        Task<List<Pet>> GetAllPetsAsync();
        Task<List<Pet>> searchPetsAsync(string searchTerm);
        Task<List<Pet>> FilterPetsAsync(PetFilterParams petFilterParams);

        Task<List<Pet>> FilterPetsAndIdAsync(PetFilterParams petFilterParams, int[] petIds);
        Task<List<Pet>> GetPetsByIdsAsync(int[] petIds);
    }
}
