using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads
{
    public class UserInsurancesPayload
    {
        public UserInsurancesPayload()
        {
            this.Insurances = new List<InsuranceModel>();
        }
        public List<InsuranceModel>? Insurances { get; set; }
    }
}
