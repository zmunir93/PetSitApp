using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PetSitApp.Models;

public partial class PetSitAppContext : DbContext
{
    public PetSitAppContext()
    {
    }

    public PetSitAppContext(DbContextOptions<PetSitAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DaysUnavailable> DaysUnavailables { get; set; }

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetPicture> PetPictures { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<ServiceType> ServiceTypes { get; set; }

    public virtual DbSet<Sitter> Sitters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeekAvailability> WeekAvailabilities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DaysUnavailable>(entity =>
        {
            entity.ToTable("DaysUnavailable");

            entity.HasOne(d => d.Sitter).WithMany(p => p.DaysUnavailables)
                .HasForeignKey(d => d.SitterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DaysUnavailable_Sitters");
        });

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

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(e => e.JobType).HasMaxLength(20);

            entity.HasOne(d => d.Owner).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.OwnerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Owners");

            entity.HasOne(d => d.Pet).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.PetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Pets");

            entity.HasOne(d => d.Sitter).WithMany(p => p.Reservations)
                .HasForeignKey(d => d.SitterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Reservations_Sitters");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasIndex(e => e.SitterId, "IX_Services").IsUnique();

            entity.HasOne(d => d.Sitter).WithOne(p => p.Service)
                .HasForeignKey<Service>(d => d.SitterId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ServiceType>(entity =>
        {
            entity.Property(e => e.ServiceOffered).HasMaxLength(20);

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceTypes)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceTypes_Services");
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

        modelBuilder.Entity<WeekAvailability>(entity =>
        {
            entity.HasIndex(e => e.SitterId, "IX_WeekAvailabilities").IsUnique();

            entity.HasOne(d => d.Sitter).WithOne(p => p.WeekAvailability)
                .HasForeignKey<WeekAvailability>(d => d.SitterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeekAvailabilities_Sitters");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
