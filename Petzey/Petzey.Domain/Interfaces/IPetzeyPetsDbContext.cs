using Petzey.Domain.Entities;
using System.Data.Entity;

namespace Petzey.Domain.Interfaces
{
    public interface IPetzeyPetsDbContext
    {
        DbSet<Pet> Pets { get; set; }
    }
}