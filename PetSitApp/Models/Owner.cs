using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Owner
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Age { get; set; }

    public int UserId { get; set; }

    public byte[] ProfilePicture { get; set; } = null!;

    public virtual ICollection<Pet> Pets { get; set; } = new List<Pet>();

    public virtual User User { get; set; } = null!;
}
