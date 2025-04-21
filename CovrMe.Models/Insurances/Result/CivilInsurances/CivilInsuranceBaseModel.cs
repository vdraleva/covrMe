using CovrMe.Models.Insurances.Result.CivilInsurances;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.CivilInsurance
{
    public class CivilInsuranceBaseModel
    {
        public int Success { get; set; }

        public CivilInsuranceOffer? CivilInsuranceOffer { get; set; }

        public string? InsuranceCompanyName { get; set; }
    }
}
