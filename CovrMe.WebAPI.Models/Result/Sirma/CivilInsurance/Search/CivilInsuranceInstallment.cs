using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsuranceInstallment
    {
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

        [JsonProperty("number")]
        public decimal Number { get; set; }

        [JsonProperty("maturity")]
        public string? PaymentDate { get; set; }
    }
}
