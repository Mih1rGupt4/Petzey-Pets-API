using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;

namespace Petzey.Data
{
    public class PetzeyPetsDbContext : DbContext, IPetzeyPetsDbContext
    {
        public PetzeyPetsDbContext() : base("DefaultConnection")
        {

        }
        
        public DbSet<Pet> Pets { get; set; }
    }
}
