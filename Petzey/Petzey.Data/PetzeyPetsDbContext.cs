﻿using Petzey.Domain.Entities;
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
            // Use SqlAzureExecutionStrategy for transient failures
            Database.SetInitializer<PetzeyPetsDbContext>(null);
            this.Database.Initialize(false);
            this.Database.CommandTimeout = 180;
        }
        
        public DbSet<Pet> Pets { get; set; }
    }
}
