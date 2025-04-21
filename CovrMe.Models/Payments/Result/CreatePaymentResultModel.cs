using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Payments.Result
{
    public class CreatePaymentResultModel
    {
        public string? LocalOrderNumber { get; set; }
        public string? FormUrl { get; set; }
        public string? DocumentBatchId { get; set; }
        public string? StickerId { get; set; }
        public string? GreencardId { get; set; }
    }
}
