using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class DeliveryInfo
{
    public Guid Id { get; set; }

    public string Names { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string City { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public string? Street { get; set; }

    public int BlockNo { get; set; }

    public string? AdditionalInfo { get; set; }

    public int FloorNo { get; set; }

    public int ApartmentNo { get; set; }

    public Guid UserId { get; set; }

    public string? SpeedyOfficeId { get; set; }

    public virtual User User { get; set; } = null!;
}
