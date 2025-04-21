using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Payments.Result.Payloads
{
    public class PaymentStatusPayload
    {
        [JsonProperty("paymentStatus")]
        public BaseResultModel? Result { get; set; }
    }
}
