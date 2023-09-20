using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class Sitter
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
}
