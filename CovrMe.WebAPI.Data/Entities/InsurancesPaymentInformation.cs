using System;
using System.Collections.Generic;

namespace CovrMe.WebAPI.Data.Entities;

public partial class InsurancesPaymentInformation
{
    public Guid Id { get; set; }

    public Guid InsuranceId { get; set; }

    public Guid PaymentInformationId { get; set; }

    public virtual Insurance Insurance { get; set; } = null!;

    public virtual PaymentInformation PaymentInformation { get; set; } = null!;
}
