using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Payment
{
    public class CheckPaymentStatusInput
    {
        public string? LocalOrderId { get; set; }
    }
}
