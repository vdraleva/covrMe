using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Insurances.Payloads.Health
{
    public class HealthPolicyPayload
    {
        public string? PolicyNo { get; set; }
        public string? ErrorId { get; set; }
        public string? Message { get; set; }
        public int Success { get; set; }
    }
}
