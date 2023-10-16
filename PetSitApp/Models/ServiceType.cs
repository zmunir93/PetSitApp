using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class ServiceType
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public string ServiceOffered { get; set; } = null!;

    public int Rate { get; set; }

    public virtual Service Service { get; set; } = null!;
}
