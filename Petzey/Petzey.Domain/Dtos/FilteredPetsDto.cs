using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Dtos
{
    public class FilteredPetsDto
    {
        public List<Pet> Pets { get; set; }
        public int Count { get; set; }
    }
}
