using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Therapist
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int? Experience { get; set; }

    public string? Image { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual ICollection<SlotDetail> SlotDetails { get; set; } = new List<SlotDetail>();
}
