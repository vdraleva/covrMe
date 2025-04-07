using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.InstallmentPay
{
    public class CivilInsuranceInstallmentIssue
    {
        public CivilInsuranceInstallmentIssue()
        {
            this.GreenCard = new CivilInsuranceGreencard();
        }
        [JsonProperty("greencard")]
        public CivilInsuranceGreencard? GreenCard { get; set; }
    }
}
