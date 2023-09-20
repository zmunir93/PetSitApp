using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Role { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
