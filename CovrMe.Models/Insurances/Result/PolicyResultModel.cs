using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Insurances.Result
{
    public class PolicyResultModel
    {
        public string? PolicyNumber { get; set; }
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }
    }
}
