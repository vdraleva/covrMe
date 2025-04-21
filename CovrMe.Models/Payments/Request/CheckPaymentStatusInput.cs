using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Payments.Request
{
    public class CheckPaymentStatusInput
    {
        public string? LocalOrderId { get; set; }
    }
}
