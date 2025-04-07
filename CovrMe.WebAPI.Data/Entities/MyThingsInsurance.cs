using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class MyThingsInsurance
{
    public Guid Id { get; set; }

    public byte PropertyType { get; set; }

    public byte ObjectType { get; set; }

    public decimal InsuranceSum { get; set; }

    public Guid InsuranceId { get; set; }

    public string Brand { get; set; } = null!;

    public string Model { get; set; } = null!;

    public virtual Insurance Insurance { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
