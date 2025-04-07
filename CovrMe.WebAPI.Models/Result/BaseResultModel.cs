using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result
{
    public class BaseResultModel
    {
        public int Code { get; set; }
        public int ExceptionType { get; set; }
        public string? Message { get; set; }
    }
}
