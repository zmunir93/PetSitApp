using Microsoft.EntityFrameworkCore;
using PetSitApp.Models;

namespace PetSitApp.Data
{
    public class ApplicationDBContext :DbContext  
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Sitter> Sitters { get; set; }
    }
}
