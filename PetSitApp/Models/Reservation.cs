using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Reservation
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public int SitterId { get; set; }

    public int PetId { get; set; }

    public string SessionId { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string JobType { get; set; } = null!;

    public virtual Owner Owner { get; set; } = null!;

    public virtual Pet Pet { get; set; } = null!;

    public virtual Sitter Sitter { get; set; } = null!;
}
