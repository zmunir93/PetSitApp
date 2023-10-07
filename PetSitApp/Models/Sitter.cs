using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Sitter
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte[] ProfilePicture { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int Age { get; set; }

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Zip { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
