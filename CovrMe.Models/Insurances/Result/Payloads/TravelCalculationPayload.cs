using CovrMe.Models.Insurances.Result.TravelInsurance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class TravelCalculationPayload
    {
        [JsonProperty("travelCalculation")]
        public TravelCalculationResultModel? Result { get; set; }
    }
}
