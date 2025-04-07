using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class UsersInsurance
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid InsuranceId { get; set; }

    public bool IsInsurer { get; set; }

    public bool IsMainUser { get; set; }

    public bool IsUsualDriver { get; set; }

    public bool IsInsured { get; set; }

    public virtual Insurance Insurance { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
