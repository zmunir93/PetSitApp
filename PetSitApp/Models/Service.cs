using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Service
{
    public int Id { get; set; }

    public int PetType { get; set; }

    public int SitterId { get; set; }

    public virtual ICollection<ServiceType> ServiceTypes { get; set; } = new List<ServiceType>();

    public virtual Sitter Sitter { get; set; } = null!;
}
