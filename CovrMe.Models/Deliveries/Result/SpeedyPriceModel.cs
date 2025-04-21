using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Deliveries.Result
{
    public class SpeedyPriceModel
    {
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("vat")]
        public decimal Vat { get; set; }

        [JsonProperty("total")]
        public decimal Total { get; set; }
    }
}
