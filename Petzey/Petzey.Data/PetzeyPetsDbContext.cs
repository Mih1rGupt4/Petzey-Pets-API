using Petzey.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Petzey.Domain.Entities;
using Petzey.Domain.Interfaces;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.SqlServer;

namespace Petzey.Data
{
    public class PetzeyPetsDbContext : DbContext
    {
        public PetzeyPetsDbContext() : base("DefaultConnection")
        {
        }
        
        public DbSet<Pet> Pets { get; set; }
    }
}
