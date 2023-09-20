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

    public virtual DbSet<Owner> Owners { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Pet> Pets { get; set; }

    public virtual DbSet<PetPicture> PetPictures { get; set; }

    public virtual DbSet<Sitter> Sitters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=PetSitApp;Trusted_Connection=True;Encrypt=False;");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
