using CovrMe.Models.Insurances.Result.MountainInsurance;
using CovrMe.Models.Insurances.Result.MyThings;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class MyThingsCalculationPayload
    {
        [JsonProperty("myThingsCalculation")]
        public MyThingsCalculationResultModel? Result { get; set; }
    }
}
