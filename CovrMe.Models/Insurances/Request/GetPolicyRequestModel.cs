using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Request
{
    public class GetPolicyRequestModel
    {
        public string? UserId { get; set; }
        public string? InsuranceId { get; set; }
        public string? PolicyNo { get; set; }
    }
}
