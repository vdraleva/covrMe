using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.InstallmentPay
{
    public class CivilInsuranceInstallmentPayResponse
    {
        public CivilInsuranceInstallmentPayResponse()
        {
            this.Info = new CivilInsuranceInstallmentMainInfo();
        }
        [JsonProperty("success")]
        public int Success { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("result")]
        public CivilInsuranceInstallmentMainInfo? Info { get; set; }
    }
}
