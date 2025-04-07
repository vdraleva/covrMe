using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class PaymentInformation
{
    public Guid Id { get; set; }

    public string LocalOrderNumber { get; set; } = null!;

    public string DskOrderNumber { get; set; } = null!;

    public int Status { get; set; }

    public string Operation { get; set; } = null!;

    public virtual ICollection<InsurancesPaymentInformation> InsurancesPaymentInformations { get; set; } = new List<InsurancesPaymentInformation>();
}
