using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result.MountainInsurance
{
    public class MountainPolicyResultModel
    {
        public string? PolicyNo { get; set; }
        public string? ErrorId { get; set; }
        public int Success { get; set; }
        public string? Message { get; set; }
    }
}
