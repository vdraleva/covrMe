using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsurancePolicyIssue
    {
        public CivilInsurancePolicyIssue()
        {
            Policy = new CivilInsurancePolicy();
            GreenCard = new CivilInsuranceGreencard();
        }

        [JsonProperty("policy")]
        public CivilInsurancePolicy? Policy { get; set; }

        [JsonProperty("greencard")]
        public CivilInsuranceGreencard? GreenCard { get; set; }
    }
}
