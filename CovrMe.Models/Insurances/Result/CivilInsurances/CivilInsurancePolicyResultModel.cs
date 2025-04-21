using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsurancePolicyResultModel
    {
        [JsonProperty("success")]
        public int Success { get; set; }

        public string? ErrorId { get; set; }
        public string? PolicyNo { get; set; }
        public string? Message { get; set; }
    }
}
