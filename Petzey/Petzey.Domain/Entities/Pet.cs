using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Petzey.Domain.Entities
{
    public class Pet
    {
        public int PetID { get; set; }
        public string PetParentID { get; set; }
        public string PetName { get; set; }

        public string PetImage { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public bool Neutered {  get; set; }
        public DateTime DateOfBirth { get; set; }
        public string BloodGroup { get; set; }
        //public string Allergies { get; set; }
        //public DateTime? LastAppointmentDate { get; set; }
        public bool isDeleted { get; set; }
    }
}
