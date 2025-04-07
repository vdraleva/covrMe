using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsuranceFullInstallments
    {
        public CivilInsuranceFullInstallments()
        {
            this.FourthInstallment = new CivilInsuranceInstallments();
            this.SecondInstallment = new CivilInsuranceInstallments();
            this.FourthInstallment = new CivilInsuranceInstallments();
        }

        [JsonProperty("1")]
        public CivilInsuranceInstallments? FirstInstallment { get; set; }

        [JsonProperty("2")]
        public CivilInsuranceInstallments? SecondInstallment { get; set; }

        [JsonProperty("4")]
        public CivilInsuranceInstallments? FourthInstallment { get; set; }
    }
}
