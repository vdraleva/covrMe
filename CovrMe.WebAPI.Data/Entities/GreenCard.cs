using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class GreenCard
{
    public Guid Id { get; set; }

    public string GreenCardNumber { get; set; } = null!;

    public Guid DocumentsBatchId { get; set; }

    public bool IsUsed { get; set; }

    public bool? IsError { get; set; }

    public virtual ICollection<CivilInsurance> CivilInsurances { get; set; } = new List<CivilInsurance>();

    public virtual DocumentsBatch DocumentsBatch { get; set; } = null!;
}
