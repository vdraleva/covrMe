using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.TravelInsurance
{
    public class TravelInsuranceModel
    {
        public string? Id { get; set; }

        public byte TerritorialValidity { get; set; }

        public byte TravelPurpose { get; set; }

        public decimal CompensationAmount { get; set; }

        public string? InsuranceId { get; set; }
    }
}
