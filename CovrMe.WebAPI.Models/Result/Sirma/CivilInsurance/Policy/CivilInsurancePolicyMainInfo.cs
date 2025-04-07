using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Policy
{
    public class CivilInsurancePolicyMainInfo
    {
        public CivilInsurancePolicyMainInfo()
        {
            this.CivilInsuranceCalculation = new CivilInsuranceCalculationInfo();
            this.Issue = new CivilInsurancePolicyIssue();
            this.Note = new CivilInsurancePolicyNote();
        }

        [JsonProperty("calc")]
        public CivilInsuranceCalculationInfo? CivilInsuranceCalculation { get; set; }

        [JsonProperty("desc")]
        public string? Description { get; set; }

        [JsonProperty("issue")]
        public CivilInsurancePolicyIssue? Issue { get; set; }

        [JsonProperty("note")]
        public CivilInsurancePolicyNote Note { get; set; }
    }
}
