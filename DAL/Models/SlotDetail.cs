using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class SlotDetail
{
    public int Id { get; set; }

    public int SlotId { get; set; }

    public int TherapistId { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Slot Slot { get; set; } = null!;

    public virtual Therapist Therapist { get; set; } = null!;
}
