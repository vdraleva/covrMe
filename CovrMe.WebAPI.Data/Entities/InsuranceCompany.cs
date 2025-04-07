using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class InsuranceCompany
{
    public Guid Id { get; set; }

    public string CompanyName { get; set; } = null!;

    public virtual ICollection<DocumentsBatch> DocumentsBatches { get; set; } = new List<DocumentsBatch>();

    public virtual ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
}
