using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class HealthInsurance
{
    public Guid Id { get; set; }

    public byte PackageType { get; set; }

    public decimal InstallmentPrice { get; set; }

    public Guid InsuranceId { get; set; }

    public bool IsFamily { get; set; }

    public virtual Insurance Insurance { get; set; } = null!;
}
