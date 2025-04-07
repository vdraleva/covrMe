using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class TravelInsurance
{
    public Guid Id { get; set; }

    public byte TerritorialValidity { get; set; }

    public byte TripPurpose { get; set; }

    public decimal CompensationAmount { get; set; }

    public Guid InsuranceId { get; set; }

    public byte TravelPolicyType { get; set; }

    public virtual Insurance Insurance { get; set; } = null!;
}
