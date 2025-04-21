using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class CivilInsuranceInstallmentPayload
    {
        public CivilInsuranceInstallmentPayload()
        {
            this.Result = new BaseResultModel();
        }

        [JsonProperty("civilInsuranceInstallment")]
        public BaseResultModel? Result { get; set; }
    }
}
