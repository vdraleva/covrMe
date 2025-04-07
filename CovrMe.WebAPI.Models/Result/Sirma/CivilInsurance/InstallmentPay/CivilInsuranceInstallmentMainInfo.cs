using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.InstallmentPay
{
    public class CivilInsuranceInstallmentMainInfo
    {
        public CivilInsuranceInstallmentMainInfo()
        {
            this.Issue = new CivilInsuranceInstallmentIssue();
        }
        [JsonProperty("issue")]
        public CivilInsuranceInstallmentIssue? Issue { get; set; }
    }
}
