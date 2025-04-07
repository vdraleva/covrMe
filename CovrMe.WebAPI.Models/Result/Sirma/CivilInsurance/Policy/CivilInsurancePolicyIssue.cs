using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy
{
    public class CivilInsurancePolicyIssue
    {
        public CivilInsurancePolicyIssue()
        {
            this.Policy = new CivilInsurancePolicy();
            this.GreenCard = new CivilInsuranceGreencard();
        }

        [JsonProperty("policy")]
        public CivilInsurancePolicy? Policy { get; set; }

        [JsonProperty("greencard")]
        public CivilInsuranceGreencard? GreenCard { get; set; }
    }
}
