using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Slot
{
    public int Id { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public virtual ICollection<SlotDetail> SlotDetails { get; set; } = new List<SlotDetail>();
}
