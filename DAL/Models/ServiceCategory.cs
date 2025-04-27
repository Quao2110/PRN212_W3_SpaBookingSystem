using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ServiceCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Signature { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
