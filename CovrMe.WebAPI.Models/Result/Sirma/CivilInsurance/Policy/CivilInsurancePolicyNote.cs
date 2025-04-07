using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy
{
    public class CivilInsurancePolicyNote
    {
        [JsonProperty("number")]
        public string? Number { get; set; }

        [JsonProperty("pdf_url")]
        public string? Url { get; set; }
    }
}
