using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Owner
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Bio { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? Age { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Zip { get; set; }

    public byte[]? ProfilePicture { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    public virtual User User { get; set; } = null!;
}
