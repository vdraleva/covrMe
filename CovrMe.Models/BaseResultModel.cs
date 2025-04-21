using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CovrMe.Models
{
    public class BaseResultModel
    {
        public int Code { get; set; }
        public int ExceptionType { get; set; }
        public string? Message { get; set; }
        public string? ErrorId { get; set; }
    }
}
