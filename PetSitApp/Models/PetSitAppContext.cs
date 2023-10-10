using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetSitApp.Models;

public partial class PetsitAppContext : DbContext
{
    public PetsitAppContext()
    {
    }

    public PetsitAppContext(DbContextOptions<PetsitAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetPicture> PetPictures { get; set; }

    public virtual DbSet<Sitter> Sitters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Owners_UserId").IsUnique();

            entity.Property(e => e.ProfilePicture).HasDefaultValueSql("(0x)");

            entity.HasOne(d => d.User).WithOne(p => p.Owner).HasForeignKey<Owner>(d => d.UserId);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Permissions_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.Permissions).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.HasIndex(e => e.OwnerId, "IX_Pets_OwnerId");

            entity.Property(e => e.About).HasMaxLength(500);
            entity.Property(e => e.AdditionalInfo).HasMaxLength(500);
            entity.Property(e => e.Care).HasMaxLength(500);
            entity.Property(e => e.FeedingSchedule).HasMaxLength(500);
            entity.Property(e => e.MedicalInformation).HasMaxLength(500);
            entity.Property(e => e.VetInformation).HasMaxLength(500);

            entity.HasOne(d => d.Owner).WithMany(p => p.Pets).HasForeignKey(d => d.OwnerId);
        });

        modelBuilder.Entity<PetPicture>(entity =>
        {
            entity.HasOne(d => d.Pet).WithMany(p => p.PetPictures)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PetPictures_Pets");
        });

        modelBuilder.Entity<Sitter>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_Sitters").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(50);
            entity.Property(e => e.City).HasMaxLength(20);
            entity.Property(e => e.FirstName).HasMaxLength(20);
            entity.Property(e => e.LastName).HasMaxLength(20);
            entity.Property(e => e.State).HasMaxLength(15);
            entity.Property(e => e.Zip).HasMaxLength(5);

            entity.HasOne(d => d.User).WithOne(p => p.Sitter).HasForeignKey<Sitter>(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
