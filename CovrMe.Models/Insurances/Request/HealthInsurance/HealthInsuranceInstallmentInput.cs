using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.HealthInsurance
{
    public class HealthInsuranceInstallmentInput
    {
        public string? InsuranceId { get; set; }
        public string? UserId { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? Email { get; set; }
    }
}
