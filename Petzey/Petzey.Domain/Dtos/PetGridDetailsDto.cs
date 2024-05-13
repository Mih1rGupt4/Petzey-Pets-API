using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Dtos
{
    public class PetGridDetailsDto
    {
       
        public int PetID { get; set; }
        public string PetParentID { get; set; }
        public string PetName { get; set; }
        public string PetImage { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
       
    }
}
