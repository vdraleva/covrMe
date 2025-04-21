using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.HealthInsurance
{
    public class HealthInsuranceModel
    {
        public string? Id { get; set; }

        public byte PackageType { get; set; }

        public decimal InstallmentPrice { get; set; }

        public string? InsuranceId { get; set; }
        public bool IsFamily { get; set; }
    }
}
