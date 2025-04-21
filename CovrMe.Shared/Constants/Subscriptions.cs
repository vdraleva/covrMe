using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Shared.Constants
{
    public class Subscriptions
    {
        public const string PaymentStatus = "subscription($localOrderId:String!){\r\n  paymentStatus(localOrderId:  $localOrderId){\r\n    code\r\n  }\r\n}";
    }
}
