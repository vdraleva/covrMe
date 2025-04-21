using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.MountainInsurance
{
    public class MountainInsuranceModel
    {
        public string? Id { get; set; }

        public decimal CompensationAmount { get; set; }

        public bool IsExtreme { get; set; }

        public string? InsuranceId { get; set; }
    }
}
