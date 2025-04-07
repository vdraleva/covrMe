using CovrMe.WebAPI.Models.Result.Sirma.CivilInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.CivilInsurance
{
    public class CivilInsuranceLongSearchPayload
    {
        public CivilInsuranceLongSearchPayload()
        {
            InsuranceOffers = new List<CivilInsuranceBaseModel>();
        }
        public List<CivilInsuranceBaseModel> InsuranceOffers { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
    }
}
