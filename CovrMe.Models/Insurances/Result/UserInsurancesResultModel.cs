using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result
{
    public class UserInsurancesResultModel
    {
        public UserInsurancesResultModel()
        {
            this.Insurances = new List<InsuranceModel>();
        }
        public List<InsuranceModel>? Insurances { get; set; }
    }
}
