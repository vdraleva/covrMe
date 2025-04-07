using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class Insurance
{
    public Guid Id { get; set; }

    public DateTime PolicyEndDate { get; set; }

    public DateTime StartDate { get; set; }

    public byte Status { get; set; }

    public Guid InsuranceCompanyId { get; set; }

    public DateTime CreationDate { get; set; }

    public decimal Price { get; set; }

    public byte Type { get; set; }

    public Guid CurrencyId { get; set; }

    public DateTime CurrentEndDate { get; set; }

    public string PolicyNo { get; set; } = null!;

    public string? PdfUrl { get; set; }

    public int InstallmentsNumber { get; set; }

    public int InstallmentToPay { get; set; }

    public string? ReceiptUrl { get; set; }

    public virtual CivilInsurance? CivilInsurance { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual HealthInsurance? HealthInsurance { get; set; }

    public virtual InsuranceCompany InsuranceCompany { get; set; } = null!;

    public virtual ICollection<InsurancesPaymentInformation> InsurancesPaymentInformations { get; set; } = new List<InsurancesPaymentInformation>();

    public virtual MountainInsurance? MountainInsurance { get; set; }

    public virtual MyThingsInsurance? MyThingsInsurance { get; set; }

    public virtual TravelInsurance? TravelInsurance { get; set; }

    public virtual ICollection<UsersInsurance> UsersInsurances { get; set; } = new List<UsersInsurance>();
}
