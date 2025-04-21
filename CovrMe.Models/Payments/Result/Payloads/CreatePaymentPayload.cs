using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Payments.Result.Payloads
{
    public class CreatePaymentPayload
    {
        [JsonProperty("createPayment")]
        public CreatePaymentResultModel PaymentInfo { get; set; }
    }
}
