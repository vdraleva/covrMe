using CovrMe.Models.Insurances.Result.MountainInsurance;
using CovrMe.Models.Insurances.Result.TravelInsurance;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class MountainPolicyPayload
    {
        public MountainPolicyPayload()
        {
            this.Result = new MountainPolicyResultModel();
        }

        [JsonProperty("mountainPolicy")]
        public MountainPolicyResultModel? Result { get; set; }
    }
}
