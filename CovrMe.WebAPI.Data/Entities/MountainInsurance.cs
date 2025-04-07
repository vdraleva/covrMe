using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class MountainInsurance
{
    public Guid Id { get; set; }

    public decimal CompensationAmount { get; set; }

    public bool IsExtreme { get; set; }

    public Guid InsuranceId { get; set; }

    public virtual Insurance Insurance { get; set; } = null!;
}
