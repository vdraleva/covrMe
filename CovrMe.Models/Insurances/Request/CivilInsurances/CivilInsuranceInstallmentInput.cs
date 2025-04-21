using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.CivilInsurances
{
    public class CivilInsuranceInstallmentInput
    {
        public string? InsuranceId { get; set; }
        public string? UserId { get; set; }
        public string? GreenCardId { get; set; }
        public string? StickerId { get; set; }
        public string? LocalOrderNumber { get; set; }
        public string? Email { get; set; }
    }
}
