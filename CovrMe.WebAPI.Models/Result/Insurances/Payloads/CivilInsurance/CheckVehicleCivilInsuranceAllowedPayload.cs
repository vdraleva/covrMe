using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.CivilInsurance
{
    public class CheckVehicleCivilInsuranceAllowedPayload
    {
        public bool IsForbidden { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
