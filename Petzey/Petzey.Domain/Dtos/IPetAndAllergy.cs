using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Dtos
{
    public class IPetAndAllergy
    {
        public Pet Pet { get; set; }
        public List<int> allergies { get; set; }
    }
}
