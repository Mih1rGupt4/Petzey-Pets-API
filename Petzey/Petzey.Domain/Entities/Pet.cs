using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Entities
{
    public class Pet
    {
        public int PetID { get; set; }
        public int PetParentID { get; set; }
        public string PetName { get; set; }

        [Column(TypeName ="varbinary(MAX)")]
        public byte[] PetImage { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Age { get; set; }
        public string BloodGroup { get; set; }
        public string Allergies { get; set; }
    }
}
