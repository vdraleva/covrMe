using CovrMe.Models.Insurances.Result.CivilInsurances;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class CivilInsuranceLongPolicyPayload
    {
        public CivilInsuranceLongPolicyPayload()
        {
            PolicyInfo = new CivilInsurancePolicyResultModel();
        }

        [JsonProperty("civilInsuranceLongPolicy")]
        public CivilInsurancePolicyResultModel PolicyInfo { get; set; }
    }
}
