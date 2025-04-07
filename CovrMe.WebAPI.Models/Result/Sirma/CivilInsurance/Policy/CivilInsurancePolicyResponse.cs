using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy
{
    public class CivilInsurancePolicyResponse
    {
        public CivilInsurancePolicyResponse()
        {
            this.Info = new CivilInsurancePolicyMainInfo();
        }

        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }

        [JsonProperty("result")]
        public CivilInsurancePolicyMainInfo? Info { get; set; }
    }
}
