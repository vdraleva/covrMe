using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Insurances.Mountain
{
    public class MountainCalculationInput
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int InsuranceSum { get; set; }
        public bool IsExtreme { get; set; }
        public byte NumberUnder18 { get; set; } //NumberAge1
        public byte NumberOver18 { get; set; } //NumberAge2
    }
}
