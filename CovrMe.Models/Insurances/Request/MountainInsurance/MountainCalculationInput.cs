using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request.MountainInsurance
{
    public class MountainCalculationInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InsuranceSum { get; set; }
        public bool IsExtreme { get; set; }
        public byte NumberUnder18 { get; set; }
        public byte NumberOver18 { get; set; } 
    }
}
