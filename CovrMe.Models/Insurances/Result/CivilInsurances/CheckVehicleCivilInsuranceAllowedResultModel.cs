using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.CivilInsurances
{
    public class CheckVehicleCivilInsuranceAllowedResultModel
    {
        public bool IsForbidden { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
