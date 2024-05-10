using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Entities
{
    public class PetAllergies
    {
        public int Id { get; set; }
        public int PetID {  get; set; }
        public int AllergyId { get; set; }
        public virtual Pet Pet { get; set; }
        public virtual Allergy Allergy { get; set; }

    }
}
