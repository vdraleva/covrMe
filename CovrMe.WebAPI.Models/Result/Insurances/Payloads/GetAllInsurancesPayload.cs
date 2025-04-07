using CovrMe.WebAPI.Models.Result.InsuranceCompanies;
using CovrMe.WebAPI.Models.Result.Insurances;
using CovrMe.WebAPI.Models.Result.Vehicles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads
{
    public class GetAllInsurancesPayload
    {
        public GetAllInsurancesPayload()
        {
            this.Insurances = new List<InsuranceModel>();
        }
        public List<InsuranceModel>? Insurances { get; set; }
    }
}
