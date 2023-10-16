using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class DaysUnavailable
{
    public int Id { get; set; }

    public int SitterId { get; set; }

    public bool IsAvailable { get; set; }

    public DateTime Date { get; set; }

    public virtual Sitter Sitter { get; set; } = null!;
}
