using CovrMe.Models.Insurances.CivilInsurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CivilInsuranceSearchResultModel
    {
        public CivilInsuranceSearchResultModel()
        {
            InsuranceOffers = new List<CivilInsuranceBaseModel>();
        }
        public List<CivilInsuranceBaseModel> InsuranceOffers { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }

    }
}
