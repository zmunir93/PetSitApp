using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Owner? Owner { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
