using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Payment.Payloads
{
    public class CreatePaymentPayload
    {
        public string? LocalOrderNumber { get; set; }
        public string? FormUrl { get; set; }
        public string? DocumentBatchId { get; set; }
        public string? StickerId { get; set; }
        public string? GreencardId{ get; set; }
    }
}
