using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class CivilInsurance
{
    public Guid Id { get; set; }

    public decimal FirstInstallmentPrice { get; set; }

    public decimal SecondInstallmentPrice { get; set; }

    public decimal ThirdInstallmentPrice { get; set; }

    public decimal FourthInstallmentPrice { get; set; }

    public Guid? GreenCardId { get; set; }

    public Guid? StickerId { get; set; }

    public Guid InsuranceId { get; set; }

    public DateTime? SecondInstallmentDate { get; set; }

    public DateTime? ThirdInstallmentDate { get; set; }

    public DateTime? FourthInstallmentDate { get; set; }

    public string? GreenCardPdfUrl { get; set; }

    public Guid? VehicleId { get; set; }

    public decimal FirstInstallmentTax { get; set; }

    public decimal SecondInstallmentTax { get; set; }

    public decimal ThirdInstallmentTax { get; set; }

    public decimal FourthInstallmentTax { get; set; }

    public string? VehicleModel { get; set; }

    public string? VehicleBrand { get; set; }

    public string? VehiclePlateNumber { get; set; }

    public virtual GreenCard? GreenCard { get; set; }

    public virtual Insurance Insurance { get; set; } = null!;

    public virtual Sticker? Sticker { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}
