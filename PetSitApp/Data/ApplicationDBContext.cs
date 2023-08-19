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
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Owner)
                .WithOne(o => o.User)
                .HasForeignKey<Owner>(o => o.UserId);
        }
    }
}
