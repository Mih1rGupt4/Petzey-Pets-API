using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Dtos
{
    public class CardPetDetailsDto
    {
        public int PetID { get; set; }
        public string PetName { get; set; }
        public string PetAge { get; set; }
        public string PetGender { get; set; }
        public int OwnerID { get; set; }
        public string petImage { get; set; }
    }
}
