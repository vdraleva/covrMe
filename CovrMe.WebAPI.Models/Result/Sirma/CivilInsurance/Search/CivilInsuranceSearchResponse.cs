using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsuranceSearchResponse
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("result")]
        public CivilInsurancesInfoModel? CivilInsurances { get; set; }

        [JsonProperty("error")]
        public string? Error { get; set; }
    }
}
