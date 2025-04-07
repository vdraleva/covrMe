using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance.Search
{
    public class CivilInsuranceInstallments
    {
        public CivilInsuranceInstallments()
        {
            this.FourthInstallment = new CivilInsuranceInstallment();
            this.SecondInstallment = new CivilInsuranceInstallment();
            this.ThirdInstallment = new CivilInsuranceInstallment();
            this.FourthInstallment = new CivilInsuranceInstallment();
        }

        [JsonProperty("1")]
        public CivilInsuranceInstallment? FirstInstallment { get; set; }

        [JsonProperty("2")]
        public CivilInsuranceInstallment? SecondInstallment { get; set; }

        [JsonProperty("3")]
        public CivilInsuranceInstallment? ThirdInstallment { get; set; }

        [JsonProperty("4")]
        public CivilInsuranceInstallment? FourthInstallment { get; set; }
    }
}
