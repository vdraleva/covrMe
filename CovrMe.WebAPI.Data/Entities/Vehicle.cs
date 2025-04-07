using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class Vehicle
{
    public Guid Id { get; set; }

    public string RegistrationCertificateNumber { get; set; } = null!;

    public string PlateNumber { get; set; } = null!;

    public Guid UserId { get; set; }

    public DateTime? FirstRegistrationDate { get; set; }

    public string? Vin { get; set; }

    public int? EngineVolume { get; set; }

    public byte? BodyType { get; set; }

    public byte? Places { get; set; }

    public byte? Color { get; set; }

    public byte? EngineType { get; set; }

    public int? NetWeight { get; set; }

    public int? GrossWeight { get; set; }

    public int? VehicleKilowatts { get; set; }

    public byte? SteeringWheel { get; set; }

    public int ModelId { get; set; }

    public int BrandId { get; set; }

    public int VehicleTypeId { get; set; }

    public int VehicleUsageId { get; set; }

    public string Model { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public string VehicleType { get; set; } = null!;

    public string VehicleUsage { get; set; } = null!;

    public int? BatteryCapacity { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<CivilInsurance> CivilInsurances { get; set; } = new List<CivilInsurance>();

    public virtual User User { get; set; } = null!;
}
