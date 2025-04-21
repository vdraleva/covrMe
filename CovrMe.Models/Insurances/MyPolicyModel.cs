using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances
{
    public class MyPolicyModel
    {
        public string? InsuranceId { get; set; }

        public string? UserId { get; set; }
        public byte InsuranceType { get; set; }
        public string? PolicyNo { get; set; }
        public string? Name { get; set; }
    }
}
