using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsuranceCalculationInfo
    {
        public CivilInsuranceCalculationInfo()
        {
            this.Installments = new CivilInsuranceInstallments();
        }

        [JsonProperty("premium_with_tax")]
        public decimal PremiumWithTax { get; set; }

        [JsonProperty("premium")]
        public decimal Premium { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("gf")]
        public decimal Gf { get; set; }

        [JsonProperty("nbbaz")]
        public decimal Nbbaz { get; set; }

        [JsonProperty("offer")]
        public string? Offer { get; set; }

        [JsonProperty("installments")]
        public CivilInsuranceInstallments? Installments { get; set; }

        [JsonProperty("full_installments_breakdown")]
        public CivilInsuranceFullInstallments? FullInstallmentsBreakdown { get; set; }
    }
}
