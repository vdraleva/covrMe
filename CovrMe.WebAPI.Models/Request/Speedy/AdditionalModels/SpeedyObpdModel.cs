using CovrMe.WebAPI.Shared.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.WebAPI.Models.Request.Speedy.AdditionalModels
{
    public class SpeedyObpdModel
    {
        [JsonProperty("option")]
        public string? Option { get; set; } = GlobalConstants.AdditionalServicesOption;

        [JsonProperty("returnShipmentServiceId")]
        public int ReturnShipmentServiceId { get; set; } = GlobalConstants.ServiceId;

        [JsonProperty("returnShipmentPayer")]
        public string? ReturnShipmentPayer { get; set; } = GlobalConstants.ReturnShipmentPayer;
    }
}
