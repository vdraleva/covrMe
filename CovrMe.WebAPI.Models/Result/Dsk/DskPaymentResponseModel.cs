using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Dsk
{
    public class DskPaymentResponseModel
    {
        [JsonProperty("orderId")]
        public string? OrderId { get; set; }

        [JsonProperty("formUrl")]
        public string? FormUrl { get; set; }
    }
}
