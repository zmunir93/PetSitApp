using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Pet
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Species { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string Breed { get; set; } = null!;

    public bool Chipped { get; set; }

    public bool Spayed { get; set; }

    public bool Friendly { get; set; }

    public DateTime AdoptionDate { get; set; }

    public string? About { get; set; }

    public string? Care { get; set; }

    public bool PottyTrained { get; set; }

    public string? EnergyLevel { get; set; }

    public string? FeedingSchedule { get; set; }

    public string? LeftAlone { get; set; }

    public string? MedicalInformation { get; set; }

    public string? AdditionalInfo { get; set; }

    public string? VetInformation { get; set; }

    public virtual Owner Owner { get; set; } = null!;

    public virtual ICollection<PetPicture> PetPictures { get; set; } = new List<PetPicture>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
