using CovrMe.Models.Insurances.Result.CivilInsurances;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class CivilInsurancePolicyPayload
    {
        public CivilInsurancePolicyPayload()
        {
            PolicyInfo = new CivilInsurancePolicyResultModel();
        }

        [JsonProperty("civilInsurancePolicy")]
        public CivilInsurancePolicyResultModel PolicyInfo { get; set; }
    }
}
