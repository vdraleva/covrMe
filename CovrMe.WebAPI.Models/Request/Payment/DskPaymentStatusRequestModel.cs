using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Payment
{
    public class DskPaymentStatusRequestModel
    {
        [JsonProperty("mdOrder")]
        public string? MdOrder { get; set; }

        [JsonProperty("orderNumber")]
        public string? OrderNumber { get; set; }

        [JsonProperty("operation")]
        public string? Operation { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }     
    }
}
