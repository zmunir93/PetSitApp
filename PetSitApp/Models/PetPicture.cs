using System;
using System.Collections.Generic;

namespace PetSitApp.Models;

public partial class PetPicture
{
    public int Id { get; set; }

    public byte[] Picture { get; set; } = null!;

    public int PetId { get; set; }

    public virtual Pet Pet { get; set; } = null!;
}
