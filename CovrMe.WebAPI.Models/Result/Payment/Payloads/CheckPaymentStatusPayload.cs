using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Payment.Payloads
{
    public class CheckPaymentStatusPayload
    {
        public string? LocalOrderNumber { get; set; }
        public int Status { get; set; }
        public string? Id { get; set; }
        public string? Operation { get; set; }
    }
}
