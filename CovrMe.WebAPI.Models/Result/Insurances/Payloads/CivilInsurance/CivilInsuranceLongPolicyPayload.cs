using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.CivilInsurance
{
    public class CivilInsuranceLongPolicyPayload
    {
        [JsonProperty("success")]
        public int Success { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
        public string? PolicyNo { get; set; }
    }
}
