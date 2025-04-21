using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Payments.Result
{
    public class CheckPaymentStatusResultModel
    {
        public string? LocalOrderNumber { get; set; }
        public int Status { get; set; }
        public string? Id { get; set; }
        public string? Operation { get; set; }
    }
}
