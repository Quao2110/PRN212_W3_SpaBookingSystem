using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public int Duration { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string? Thumbnail { get; set; }

    public int CategoryId { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ServiceCategory Category { get; set; } = null!;
}
