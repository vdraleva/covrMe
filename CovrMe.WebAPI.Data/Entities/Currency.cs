using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class Currency
{
    public Guid Id { get; set; }

    public string Code { get; set; } = null!;

    public virtual ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
}
