using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance
{
    public class CivilInsuranceBaseModel
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("system_message")]
        public string? SystemMessage { get; set; }

        [JsonProperty("result")]
        public CivilInsuranceOffer? CivilInsuranceOffer { get; set; }
        public string? InsuranceCompanyName { get; set; }
    }
}
