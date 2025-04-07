using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsuranceOffer
    {
        [JsonProperty("calc")]
        public CivilInsuranceCalculationInfo CivilInsuranceCalculation { get; set; }

        [JsonProperty("desc")]
        public string? Description { get; set; }
    }
}
