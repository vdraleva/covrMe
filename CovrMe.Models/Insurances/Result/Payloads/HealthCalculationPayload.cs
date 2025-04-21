using CovrMe.Models.Insurances.Result.HealthInsurance;
using CovrMe.Models.Insurances.Result.MountainInsurance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class HealthCalculationPayload
    {
        [JsonProperty("healthCalculation")]
        public HealthCalculationResultModel? Result { get; set; }
    }
}
