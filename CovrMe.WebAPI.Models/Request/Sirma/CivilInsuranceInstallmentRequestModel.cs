using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Sirma
{
    public class CivilInsuranceInstallmentRequestModel
    {
        public CivilInsuranceInstallmentRequestModel()
        {
            this.Type = "note";
            this.InsuranceType = "GO";
            this.Office = "1";
        }

        [JsonProperty("insurer")]
        public string? Insurer { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("insurance_type")]
        public string? InsuranceType { get; set; }

        [JsonProperty("owner_ein")]
        public string? UiNumber { get; set; }
                  
        [JsonProperty("owner_person_type")]
        public int OwnerPersonType { get; set; }

        [JsonProperty("upn")]
        public string? PolicyNo { get; set; }

        [JsonProperty("number")]
        public int InstallmentToPay { get; set; }

        [JsonProperty("sticker")]
        public string? StickerNo { get; set; }

        [JsonProperty("office")]
        public string? Office { get; set; }

        [JsonProperty("padeg")]
        public string? Maturity { get; set; }

        [JsonProperty("tax")]
        public decimal Tax { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("greencard_number")]
        public string? GreencardNo { get; set; }

        [JsonProperty("installments")]
        public int Installments { get; set; }

    }
}
