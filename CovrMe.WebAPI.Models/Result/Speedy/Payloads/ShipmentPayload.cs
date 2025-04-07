using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Result.Speedy.Payloads
{
    public class ShipmentPayload
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("deliveryDeadline")]
        public DateTime? DeliveryDeadline { get; set; }
    }
}
