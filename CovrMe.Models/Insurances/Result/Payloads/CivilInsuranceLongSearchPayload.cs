using CovrMe.Models.Insurances.Result.CivilInsurances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.Payloads
{
    public class CivilInsuranceLongSearchPayload    
    {
        public CivilInsuranceLongSearchPayload()
        {
            this.CivilInsuranceLongSearch = new CivilInsuranceSearchResultModel();
        }
        public CivilInsuranceSearchResultModel? CivilInsuranceLongSearch { get; set; }
    }
}
