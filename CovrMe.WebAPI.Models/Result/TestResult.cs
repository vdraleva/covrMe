using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result
{
    public class TestResult
    {
        public int TestNumber { get; set; }
        public bool Success { get; set; }
        public long ElapsedMilliseconds { get; set; }
        public int ResponseCode { get; set; }
        public string ApiResponse { get; set; }
    }
}
