using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using PetSitApp.Models;
namespace PetSitApp.Data;

public partial class ApplicationDBContext : DbContext
{
    public ApplicationDBContext()
    {
    }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
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
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.MedicalInformation).HasMaxLength(500);
            entity.Property(e => e.VetInformation).HasMaxLength(500);

            entity
            .HasOne(d => d.Owner)
            .WithMany(p => p.Pets)
            .HasForeignKey(d => d.OwnerId);
        });

        modelBuilder.Entity<PetPicture>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();

            entity
            .HasOne(pp => pp.Pet)
            .WithMany(p => p.PetPictures)
            .HasForeignKey(pp => pp.PetId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
