using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class HealthInsuranceInstallmentPayload
    {
        public HealthInsuranceInstallmentPayload()
        {
            this.Result = new BaseResultModel();
        }

        [JsonProperty("healthInsuranceInstallment")]
        public BaseResultModel? Result { get; set; }
    }
}
