using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class WeekAvailability
{
    public int Id { get; set; }

    public int SitterId { get; set; }

    public bool Monday { get; set; }

    public bool Tuesday { get; set; }

    public bool Wednesday { get; set; }

    public bool Thursday { get; set; }

    public bool Friday { get; set; }

    public bool Saturday { get; set; }

    public bool Sunday { get; set; }

    public virtual Sitter Sitter { get; set; } = null!;
}
