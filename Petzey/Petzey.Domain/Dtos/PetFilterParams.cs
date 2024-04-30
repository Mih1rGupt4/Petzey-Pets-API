using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Dtos
{
    public class PetFilterParams
    {
        public string PetName { get; set; }
        public string Species { get; set; }
        public int[] PetIds { get; set; }
    }
}
